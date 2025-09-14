using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.ScriptableObjects;
using TreeFlow.Runtime.Nodes.Composite;
using TreeFlow.Runtime.Nodes.Decorator;
using TreeFlow.Runtime.Nodes.Leaf;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor.UIElements
{
    /// <summary>
    ///     Element used to display and modify <see cref="BehaviorTreeAsset" />
    /// </summary>
    internal class TreeGraphView : GraphView
    {
        private BehaviorTreeAsset treeAsset;

        public delegate void TreeChanged();
        public TreeChanged OnTreeChanged;

        public TreeGraphView()
        {
            CreateUI();

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        #region UI

        public new class UxmlFactory : UxmlFactory<TreeGraphView, UxmlTraits> { }

        private void CreateUI()
        {
            var background = new GridBackground();
            background.AddToClassList("grid");
            background.name = "Background";
            Insert(0, background);
        }

        #endregion

        #region Input

        /// <inheritdoc />
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var mousePos = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

            evt.menu.AppendAction("Composite/Sequence", _ => CreateNode<Sequence>(mousePos));
            evt.menu.AppendAction("Composite/Selector", _ => CreateNode<Selector>(mousePos));
            evt.menu.AppendAction("Composite/Parallel", _ => CreateNode<Parallel>(mousePos));

            evt.menu.AppendAction("Decorator/Inverter", _ => CreateNode<Inverter>(mousePos));

            evt.menu.AppendAction("Leaf/Action", _ => CreateNode<Action>(mousePos));
            evt.menu.AppendAction("Leaf/Condition", _ => CreateNode<Condition>(mousePos));
        }

        /// <summary>
        ///     Called when the graph has changed
        /// </summary>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // Nodes moved
            if (graphViewChange.movedElements != null)
            {
                var newPositions = new Dictionary<NodeAsset, Vector2>();

                foreach (var element in graphViewChange.movedElements)
                {
                    if (element is not NodeView nodeView)
                        continue;

                    newPositions.TryAdd(nodeView.Node, nodeView.GetPosition().position);
                }

                MoveNodes(newPositions);
            }

            // Links added
            if (graphViewChange.edgesToCreate != null)
            {
                var newLinks = new List<KeyValuePair<NodeAsset, NodeAsset>>();

                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    if (edge.input.node is not NodeView input || edge.output.node is not NodeView output)
                        continue;

                    newLinks.Add(new KeyValuePair<NodeAsset, NodeAsset>(output.Node, input.Node));
                }

                CreateLinks(newLinks);
            }

            // Nodes removed
            // ReSharper disable once InvertIf
            if (graphViewChange.elementsToRemove != null)
            {
                var nodesToRemove = new List<NodeAsset>();
                var edgesToRemove = new List<KeyValuePair<NodeAsset, NodeAsset>>();

                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is NodeView nodeView)
                        nodesToRemove.Add(nodeView.Node);
                    else if (element is Edge edge)
                    {
                        if (edge.input.node is not NodeView input || edge.output.node is not NodeView output)
                            continue;

                        edgesToRemove.Add(new KeyValuePair<NodeAsset, NodeAsset>(output.Node, input.Node));
                    }
                }

                RemoveNodes(nodesToRemove);
                RemoveLinks(edgesToRemove);
            }

            return graphViewChange;
        }

        /// <summary>
        ///     Called when the view has changed
        /// </summary>
        private void OnViewTransformChanged(GraphView graphView)
        {
            treeAsset?.SaveViewport(graphView.contentViewContainer.transform);

            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <inheritdoc />
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var validPorts = new List<Port>();

            foreach (var port in ports)
            {
                if (port.direction == startPort.direction)
                    continue;

                if (port.node == startPort.node)
                    continue;

                validPorts.Add(port);
            }

            return validPorts;
        }

        #endregion

        #region Tree

        /// <summary>
        ///     Populates this view with the given <see cref="BehaviorTreeAsset" />
        /// </summary>
        public void PopulateView(BehaviorTreeAsset tree)
        {
            tree.Compute();

            graphViewChanged -= OnGraphViewChanged;
            viewTransformChanged -= OnViewTransformChanged;

            DeleteElements(graphElements);

            treeAsset = tree;

            var viewsByGuid = new Dictionary<string, INodeView>();

            foreach (var node in treeAsset.Nodes)
                viewsByGuid.TryAdd(node.GUID, AddNodeToGraph(node));

            foreach (var node in treeAsset.Nodes)
            {
                if (!viewsByGuid.TryGetValue(node.GUID, out var parentView))
                    continue;

                if (node is not IParentNode parentNode)
                    continue;

                foreach (var childNode in parentNode.Children)
                {
                    if (childNode == null)
                        continue;

                    if (!viewsByGuid.TryGetValue(childNode, out var childView))
                        continue;

                    var edge = childView.ConnectTo(parentView);

                    if (edge == null)
                        continue;

                    AddElement(edge);
                }
            }

            treeAsset.LoadViewport(contentViewContainer.transform);

            graphViewChanged += OnGraphViewChanged;
            viewTransformChanged += OnViewTransformChanged;
        }

        /// <summary>
        ///     Adds the given <see cref="NodeAsset" /> to the graph
        /// </summary>
        private INodeView AddNodeToGraph(NodeAsset graphNode)
        {
            var view = new NodeView(this);
            view.Assign(graphNode);

            AddElement(view);
            return view;
        }

        #endregion

        #region Nodes

        /// <summary>
        ///     Creates a brand new <see cref="NodeAsset" /> from the given information
        /// </summary>
        private void CreateNode<T>(Vector2 position) where T : Runtime.Core.Node
        {
            var newNode = treeAsset.AddNode<T>();

            if (newNode == null)
                return;
            
            newNode.Position = position;

            AddNodeToGraph(newNode);

            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        ///     Removes the given nodes from the graph
        /// </summary>
        private void RemoveNodes(List<NodeAsset> nodesToRemove)
        {
            if (nodesToRemove.Count == 0)
                return;

            treeAsset?.RemoveNodes(nodesToRemove);

            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        ///     Moves the given nodes at the given positions
        /// </summary>
        private void MoveNodes(Dictionary<NodeAsset, Vector2> positionsByNode)
        {
            if (positionsByNode.Count == 0)
                return;

            treeAsset?.MoveNodes(positionsByNode);

            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        ///     Renames the given node to the given name
        /// </summary>
        public void RenameNode(NodeAsset graphNode, string newName)
        {
            graphNode.Name = newName;

            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        ///     Creates brand-new links between the given nodes
        /// </summary>
        private void CreateLinks(List<KeyValuePair<NodeAsset, NodeAsset>> links)
        {
            if (links.Count == 0)
                return;

            var linksPerNode = new Dictionary<NodeAsset, ISet<NodeAsset>>();

            foreach (var (start, end) in links)
            {
                linksPerNode.TryAdd(start, new HashSet<NodeAsset>());
                linksPerNode[start].Add(end);
            }

            treeAsset?.AddLinks(linksPerNode);

            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        ///     Removes the links between the given nodes
        /// </summary>
        private void RemoveLinks(List<KeyValuePair<NodeAsset, NodeAsset>> links)
        {
            if (links.Count == 0)
                return;

            var linksPerNode = new Dictionary<NodeAsset, ISet<NodeAsset>>();

            foreach (var (start, end) in links)
            {
                linksPerNode.TryAdd(start, new HashSet<NodeAsset>());
                linksPerNode[start].Add(end);
            }

            treeAsset?.RemoveLinks(linksPerNode);

            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        #endregion
    }
}