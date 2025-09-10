using System.Collections.Generic;
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
            evt.menu.AppendAction("Decorator/Repeat", _ => CreateNode<RepeatNodeAsset>(mousePos));

            evt.menu.AppendAction("Leaf/Action", _ => CreateNode<ActionNodeAsset>(mousePos));
            evt.menu.AppendAction("Leaf/Condition", _ => CreateNode<ConditionNodeAsset>(mousePos));
        }
        
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // Nodes moved
            if (graphViewChange.movedElements != null)
            {
                var newPositions = new Dictionary<string, Vector2>();

                foreach (var element in graphViewChange.movedElements)
                {
                    if (element is not NodeView nodeView)
                        continue;

                    newPositions.TryAdd(nodeView.Node.GUID, nodeView.GetPosition().position);
                }
                
                if (newPositions.Count > 0)
                    MoveNodes(newPositions);
            }
            
            // Links added
            if (graphViewChange.edgesToCreate != null)
            {
                var newLinks = new List<KeyValuePair<string, string>>();

                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    if (edge.input.node is not NodeView input || edge.output.node is not NodeView output)
                        continue;
                    
                    newLinks.Add(new KeyValuePair<string, string>(output.Node.GUID, input.Node.GUID));
                }

                if (newLinks.Count > 0)
                    CreateLinks(newLinks);
            }

            // Nodes removed
            // ReSharper disable once InvertIf
            if (graphViewChange.elementsToRemove != null)
            {
                var nodesToRemove = new List<NodeView>();

                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is not NodeView nodeView)
                        continue;
                    
                    nodesToRemove.Add(nodeView);
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

            foreach (var node in treeAsset.Nodes)
                AddNodeToGraph(node);
            
            graphViewChanged += OnGraphViewChanged;
        }
        
        /// <summary>
        /// Adds the given <see cref="NodeAsset"/> to the graph
        /// </summary>
        private void AddNodeToGraph(NodeAsset graphNode) => AddElement(new NodeView(graphNode, this));

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
        private void RemoveNodes(List<NodeView> nodeViews)
        {
            var guids = new HashSet<string>();

            foreach (var nodeView in nodeViews)
                guids.Add(nodeView.Node.GUID);
            treeAsset?.RemoveNodes(guids);
            
            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        /// <summary>
        /// Moves the given nodes at the given positions
        /// </summary>
        private void MoveNodes(Dictionary<string, Vector2> positions)
        {
            treeAsset?.SetPositions(positions);
            
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
        private void CreateLinks(List<KeyValuePair<string, string>> links)
        {
            var linksPerNode = new Dictionary<string, HashSet<string>>();

            foreach (var (start, end) in links)
            {
                linksPerNode.TryAdd(start, new HashSet<string>());
                linksPerNode[start].Add(end);
            }
            
            treeAsset?.AddLinks(linksPerNode);
            
            EditorUtility.SetDirty(treeAsset);
            OnTreeChanged?.Invoke();
        }

        #endregion
    }
}