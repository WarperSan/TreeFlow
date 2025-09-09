using System.Collections.Generic;
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
            
            this.AddManipulator(new ContentZoomer());
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
            evt.menu.AppendAction("Test", action =>
            {
                CreateNode(action.eventInfo.mousePosition);
            });
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
                var nodesToRemove = new List<GraphNode>();

                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is not NodeView nodeView)
                        continue;
                    
                    nodesToRemove.Add(nodeView.Node);
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

        /// <summary>
        /// Creates a brand new <see cref="GraphNode"/> from the given information
        /// </summary>
        private void CreateNode(Vector2 position)
        {
            var newNode = new GraphNode
            {
                GUID = GUID.Generate().ToString(),
                Position = position
            };
            
            Undo.RecordObject(serializedTree.targetObject, "Created new Node");
            treeAsset.Nodes.Add(newNode);
            serializedTree.Update();
            
            AddNodeToGraph(newNode);
        }

        /// <summary>
        /// Removes the given nodes from the graph
        /// </summary>
        private void RemoveNodes(List<GraphNode> graphNodes)
        {
            if (graphNodes.Count == 0)
                return;
            
            Undo.RecordObject(serializedTree.targetObject, "Removed Nodes");
            foreach (var graphNode in graphNodes)
            {
                treeAsset.Nodes.Remove(graphNode);
                nodeViewsByGuid.Remove(graphNode.GUID);
            }
            serializedTree.Update();
        }

        /// <summary>
        /// Moves the given nodes at the given positions
        /// </summary>
        private void MoveNodes(Dictionary<string, Vector2> positions)
        {
            if (positions.Count == 0)
                return;
            
            Undo.RecordObject(serializedTree.targetObject, "Moved Nodes");
            foreach (var (guid, position) in positions)
            {
                if (!nodeViewsByGuid.TryGetValue(guid, out var nodeView))
                    continue;

                nodeView.Node.Position = position;
            }
            serializedTree.Update();
        }

        /// <summary>
        /// Adds the given <see cref="GraphNode"/> to the graph
        /// </summary>
        private void AddNodeToGraph(GraphNode graphNode)
        {
            var node = new NodeView(graphNode, this);
            
            nodeViewsByGuid.Add(graphNode.GUID, node);

            AddElement(node);
        }

        /// <summary>
        /// Renames the given node to the given name
        /// </summary>
        public void RenameNode(GraphNode graphNode, string newName)
        {
            Undo.RecordObject(serializedTree.targetObject, "Renamed Node");
            graphNode.Name = newName;
            serializedTree.Update();
        }

        #endregion
    }
}