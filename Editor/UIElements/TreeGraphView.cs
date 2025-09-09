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

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Test", action =>
            {
                CreateNode(action.eventInfo.mousePosition);
            });
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

        #region Tree

        private SerializedObject serializedTree;
        private BehaviorTreeAsset treeAsset;

        private readonly Dictionary<string, NodeView> nodeViewsByGuid = new();
        
        /// <summary>
        /// Assigns the tree to display to the given <see cref="BehaviorTreeAsset"/>
        /// </summary>
        public void AssignTree(SerializedObject obj)
        {
            serializedTree = obj;
            treeAsset = (BehaviorTreeAsset)serializedTree.targetObject;
        }

        /// <summary>
        /// Creates a brand new <see cref="GraphNode"/> from the given information
        /// </summary>
        public void CreateNode(Vector2 position)
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
        /// Adds the given <see cref="GraphNode"/> to the graph
        /// </summary>
        private void AddNodeToGraph(GraphNode graphNode)
        {
            var node = new NodeView();
            node.AssignNode(graphNode);
            
            nodeViewsByGuid.Add(graphNode.GUID, node);

            AddElement(node);
        }

        #endregion
    }
}