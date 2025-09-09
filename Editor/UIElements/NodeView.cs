using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor.UIElements
{
    /// <summary>
    /// Element used to display and modify <see cref="GraphNode"/>
    /// </summary>
    internal sealed class NodeView : Node
    {
        /// <summary>
        /// Current node displayed
        /// </summary>
        public readonly GraphNode Node;
        
        private readonly TreeGraphView graphView;
        
        public NodeView(GraphNode node, TreeGraphView graphView) : base(Helpers.Resources.RelativeToAbsolute("UXML/NodeView-Normal.uxml"))
        {
            styleSheets.Add(Helpers.Resources.Load<StyleSheet>("StyleSheets/NodeView-Normal.uss"));
            
            CreateTitle();
            
            // Assign node
            Node = node;
            this.graphView = graphView;
            
            var rect = GetPosition();
            rect.position = node.Position;
            SetPosition(rect);

            var _name = node.Name;

            if (string.IsNullOrEmpty(_name))
                _name = "Node";

            Title.text = TitleInput.value = _name;
        }
        
        #region UI

        private Label Title;
        private TextField TitleInput;

        private void CreateTitle()
        {
            Title = this.Q<Label>("title");
            TitleInput = this.Q<TextField>("inputTitle");
            
            Title.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.clickCount < 2)
                    return;
                
                evt.StopImmediatePropagation();

                Title.style.display = DisplayStyle.None;
                TitleInput.style.display = DisplayStyle.Flex;
                TitleInput.Focus();
            });
            
            TitleInput.RegisterCallback<FocusOutEvent>(_ =>
            {
                Title.style.display = DisplayStyle.Flex;
                TitleInput.style.display = DisplayStyle.None;
                
                TitleInput.value = Title.text;
            });
            
            TitleInput.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode != KeyCode.Return)
                    return;

                var newName = TitleInput.value;

                if (string.IsNullOrEmpty(newName))
                    newName = null;
                
                Title.text = newName ?? "Node";
                graphView.RenameNode(Node, newName);
            });
        }

        #endregion
    }
}