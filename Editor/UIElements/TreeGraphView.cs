using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Composite;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.Nodes.Decorator;
using TreeFlow.Editor.Nodes.Leaf;
using TreeFlow.Editor.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor.UIElements
{
    /// <summary>
    /// Element used to display and modify <see cref="BehaviorTreeAsset"/>
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
            styleSheets.Add(Helpers.Resources.Load<StyleSheet>(Helpers.Resources.TREE_GRAPH_VIEW_USS));

            var background = new GridBackground();
            background.AddToClassList("grid");
            background.name = "Background";
            Insert(0, background);
        }

        #endregion

        #region Input

        /// <inheritdoc/>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var mousePos = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            
            evt.menu.AppendAction("Composite/Sequence", _ => CreateNode<SequenceNodeAsset>(mousePos));
            evt.menu.AppendAction("Composite/Selector", _ => CreateNode<SelectorNodeAsset>(mousePos));
            evt.menu.AppendAction("Composite/Parallel", _ => CreateNode<ParallelNodeAsset>(mousePos));

            evt.menu.AppendAction("Decorator/Inverter", _ => CreateNode<InverterNodeAsset>(mousePos));

            evt.menu.AppendAction("Leaf/Action", _ => CreateNode<ActionNodeAsset>(mousePos));
            evt.menu.AppendAction("Leaf/Condition", _ => CreateNode<ConditionNodeAsset>(mousePos));
        }
        
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
                
                if (newPositions.Count > 0)
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

                if (newLinks.Count > 0)
                    CreateLinks(newLinks);
            }

            // Nodes removed
            // ReSharper disable once InvertIf
            if (graphViewChange.elementsToRemove != null)
            {
                var nodesToRemove = new List<NodeAsset>();

                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is not NodeView nodeView)
                        continue;
                    
                    nodesToRemove.Add(nodeView.Node);
                }
                
                if (nodesToRemove.Count > 0)
                    RemoveNodes(nodesToRemove);
            }
            
            return graphViewChange;
        }

        /// <inheritdoc/>
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
        /// Populates this view with the given <see cref="BehaviorTreeAsset"/>
        /// </summary>
        public void PopulateView(BehaviorTreeAsset tree)
        {
            tree.Compute();
            
            graphViewChanged -= OnGraphViewChanged;

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
                    if (!viewsByGuid.TryGetValue(childNode, out var childView))
                        continue;
                    
                    var edge = childView.ConnectTo(parentView);
                    
                    if (edge == null)
                        continue;
                    
                    AddElement(edge);
                }
            }
            
            graphViewChanged += OnGraphViewChanged;
        }

        /// <summary>
        /// Adds the given <see cref="NodeAsset"/> to the graph
        /// </summary>
        private INodeView AddNodeToGraph(NodeAsset graphNode)
        {
            var view = new NodeView(graphNode, this);

            AddElement(view);
            return view;
        }

        #endregion

        #region Nodes

        /// <summary>
        /// Creates a brand new <see cref="NodeAsset"/> from the given information
        /// </summary>
        private void CreateNode<T>(Vector2 position) where T : NodeAsset, new()
        {
            var newNode = treeAsset.AddNode<T>();
            newNode.Position = position;

            AddNodeToGraph(newNode);
            
            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        /// Removes the given nodes from the graph
        /// </summary>
        private void RemoveNodes(List<NodeAsset> nodesToRemove)
        {
            treeAsset?.RemoveNodes(nodesToRemove);
            
            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        /// Moves the given nodes at the given positions
        /// </summary>
        private void MoveNodes(Dictionary<NodeAsset, Vector2> positionsByNode)
        {
            treeAsset?.MoveNodes(positionsByNode);
            
            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        /// Renames the given node to the given name
        /// </summary>
        public void RenameNode(NodeAsset graphNode, string newName)
        {
            graphNode.Name = newName;
            
            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        /// Creates brand-new links between the given nodes
        /// </summary>
        private void CreateLinks(List<KeyValuePair<NodeAsset, NodeAsset>> links)
        {
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

        #endregion
    }
}