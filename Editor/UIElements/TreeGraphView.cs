using System.Collections.Generic;
using TreeFlow.Editor.ScriptableObjects;
using TreeFlow.Editor.ScriptableObjects.Nodes.Composite;
using TreeFlow.Editor.ScriptableObjects.Nodes.Decorator;
using TreeFlow.Editor.ScriptableObjects.Nodes.Leaf;
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
        private SerializedObject serializedTree;
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

            // Nodes removed
            // ReSharper disable once InvertIf
            if (graphViewChange.elementsToRemove != null)
            {
                var nodesToRemove = new List<string>();

                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is not NodeView nodeView)
                        continue;
                    
                    nodesToRemove.Add(nodeView.Node.GUID);
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
        /// Assigns the tree to display to the given <see cref="BehaviorTreeAsset"/>
        /// </summary>
        public void AssignTree(SerializedObject obj)
        {
            graphViewChanged -= OnGraphViewChanged;

            DeleteElements(graphElements);
            
            serializedTree = obj;
            treeAsset = (BehaviorTreeAsset)serializedTree.targetObject;

            foreach (var node in treeAsset.Nodes)
                AddNodeToGraph(node);
            
            graphViewChanged += OnGraphViewChanged;
        }
        
        /// <summary>
        /// Updates the tree using <see cref="action"/>, with the given title
        /// </summary>
        private void UpdateTree(System.Action action, string actionName)
        {
            Undo.RecordObject(serializedTree.targetObject, actionName);

            action?.Invoke();

            serializedTree.Update();
            OnTreeChanged?.Invoke();
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
        private void CreateNode<T>(Vector2 position) where T : NodeAsset, new() => UpdateTree(() =>
        {
            var newNode = treeAsset.AddNode<T>();
            newNode.Position = position;

            AddNodeToGraph(newNode);
        }, "Created new Node");

        /// <summary>
        /// Removes the given nodes from the graph
        /// </summary>
        private void RemoveNodes(List<string> guids) => UpdateTree(
            () => treeAsset?.RemoveNodes(guids),
            "Removed Nodes"
        );

        /// <summary>
        /// Moves the given nodes at the given positions
        /// </summary>
        private void MoveNodes(Dictionary<string, Vector2> positions) => UpdateTree(
            () => treeAsset?.SetPositions(positions),
            "Moved Nodes"
        );

        /// <summary>
        /// Renames the given node to the given name
        /// </summary>
        public void RenameNode(NodeAsset graphNode, string newName) => UpdateTree(
            () => graphNode.Name = newName,
            "Renamed Node"
        );
        
        #endregion
    }
}