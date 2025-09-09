using System.Collections.Generic;
using TreeFlow.Editor.ScriptableObjects;
using TreeFlow.Editor.ScriptableObjects.Nodes.Composite;
//using TreeFlow.Editor.ScriptableObjects.Nodes.Decorator;
//using TreeFlow.Editor.ScriptableObjects.Nodes.Leaf;
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
            styleSheets.Add(Helpers.Resources.Load<StyleSheet>("StyleSheets/TreeGraphView.uss"));
            
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
            //evt.menu.AppendAction("Composite/Parallel", _ => CreateNode<ParallelNodeAsset>(mousePos));
            //
            //evt.menu.AppendAction("Decorator/Inverter", _ => CreateNode<InverterNodeAsset>(mousePos));
            //evt.menu.AppendAction("Decorator/Repeat", _ => CreateNode<RepeatNodeAsset>(mousePos));
            //
            //evt.menu.AppendAction("Leaf/Action", _ => CreateNode<ActionNodeAsset>(mousePos));
            //evt.menu.AppendAction("Leaf/Condition", _ => CreateNode<ConditionNodeAsset>(mousePos));
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
                
                MoveNodes(newPositions);
            }

            // Nodes removed
            if (graphViewChange.elementsToRemove != null)
            {
                var nodesToRemove = new List<string>();

                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is not NodeView nodeView)
                        continue;
                    
                    nodesToRemove.Add(nodeView.Node.GUID);
                }
                
                RemoveNodes(nodesToRemove);
            }
            
            return graphViewChange;
        }

        #endregion

        #region Tree

        private SerializedObject serializedTree;
        private BehaviorTreeAsset treeAsset;

        private readonly Dictionary<string, NodeView> nodeViewsByGuid = new();

        public delegate void TreeChanged();
        public TreeChanged OnTreeChanged;
        
        /// <summary>
        /// Assigns the tree to display to the given <see cref="BehaviorTreeAsset"/>
        /// </summary>
        public void AssignTree(SerializedObject obj)
        {
            graphViewChanged -= OnGraphViewChanged;
            
            foreach (var (_, nodeView) in nodeViewsByGuid)
                RemoveElement(nodeView);

            nodeViewsByGuid.Clear();
            
            serializedTree = obj;
            treeAsset = (BehaviorTreeAsset)serializedTree.targetObject;

            foreach (var node in treeAsset.Nodes)
                AddNodeToGraph(node);
            
            graphViewChanged += OnGraphViewChanged;
        }

        #endregion

        #region Nodes

        /// <summary>
        /// Creates a brand new <see cref="NodeAsset"/> from the given information
        /// </summary>
        private void CreateNode<T>(Vector2 position) where T : NodeAsset, new()
        {
            Undo.RecordObject(serializedTree.targetObject, "Created new Node");
            
            var newNode = treeAsset.AddNode<T>();
            newNode.Position = position;
            
            serializedTree.Update();
            OnTreeChanged.Invoke();

            AddNodeToGraph(newNode);
        }

        /// <summary>
        /// Removes the given nodes from the graph
        /// </summary>
        private void RemoveNodes(List<string> guids)
        {
            if (guids.Count == 0)
                return;
            
            Undo.RecordObject(serializedTree.targetObject, "Removed Nodes");
            treeAsset.RemoveNodes(guids);

            foreach (var guid in guids)
                nodeViewsByGuid.Remove(guid);
            
            serializedTree.Update();
            OnTreeChanged.Invoke();
        }

        /// <summary>
        /// Moves the given nodes at the given positions
        /// </summary>
        private void MoveNodes(Dictionary<string, Vector2> positions)
        {
            if (positions.Count == 0)
                return;
            
            Undo.RecordObject(serializedTree.targetObject, "Moved Nodes");
            treeAsset.SetPositions(positions);
            serializedTree.Update();
            OnTreeChanged.Invoke();
        }

        /// <summary>
        /// Adds the given <see cref="NodeAsset"/> to the graph
        /// </summary>
        private void AddNodeToGraph(NodeAsset graphNode)
        {
            var node = new NodeView(graphNode, this);
            
            nodeViewsByGuid.Add(graphNode.GUID, node);

            AddElement(node);
        }

        /// <summary>
        /// Renames the given node to the given name
        /// </summary>
        public void RenameNode(NodeAsset graphNode, string newName)
        {
            Undo.RecordObject(serializedTree.targetObject, "Renamed Node");
            graphNode.Name = newName;
            serializedTree.Update();
            OnTreeChanged.Invoke();
        }

        #endregion
    }
}