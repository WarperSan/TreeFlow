using TreeFlow.Editor.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor.UIElements
{
    /// <summary>
    /// Element used to display and modify <see cref="NodeAsset"/>
    /// </summary>
    public sealed class NodeView : Node
    {
        /// <summary>
        /// Current node displayed
        /// </summary>
        public readonly NodeAsset Node;
        
        private readonly TreeGraphView graphView;
        
        internal NodeView(NodeAsset node, TreeGraphView graphView) : base(Helpers.Resources.RelativeToAbsolute("UXML/NodeView.uxml"))
        {
            styleSheets.Add(Helpers.Resources.Load<StyleSheet>("StyleSheets/NodeView.uss"));
            
            CreateHeader();
            
            // Assign node
            Node = node;
            this.graphView = graphView;
            
            var rect = GetPosition();
            rect.position = node.Position;
            SetPosition(rect);

            node.Customize(this);
        }
        
        #region Header

        private VisualElement Header;
        private TextField TitleInput;
        private VisualElement HeaderBackground;

        private void CreateHeader()
        {
            Header = this.Q("header");
            
            HeaderBackground = Header.Q<VisualElement>("background");

            var titleLabel = Header.Q<Label>("title-label");
            TitleInput = Header.Q<TextField>("title-input");

            titleLabel.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.clickCount < 2)
                    return;
                
                evt.StopImmediatePropagation();

                titleLabel.style.display = DisplayStyle.None;
                TitleInput.style.display = DisplayStyle.Flex;
                TitleInput.Focus();
            });
            
            TitleInput.RegisterCallback<FocusOutEvent>(_ =>
            {
                titleLabel.style.display = DisplayStyle.Flex;
                TitleInput.style.display = DisplayStyle.None;
                
                TitleInput.value = titleLabel.text;
            });
            
            TitleInput.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode != KeyCode.Return && evt.keyCode != KeyCode.KeypadEnter)
                    return;

                var newName = TitleInput.value;

                if (string.IsNullOrEmpty(newName))
                    newName = null;
                
                title = newName ?? "Node";
                graphView.RenameNode(Node, newName);
            });
        }

        /// <summary>
        /// Sets the title of this node
        /// </summary>
        public void SetTitle(string newTitle) => title = TitleInput.value = newTitle;

        /// <summary>
        /// Sets the title to <see cref="defaultTitle"/> if this node has no name defined
        /// </summary>
        public void SetDefaultTitle(string defaultTitle)
        {
            var _title = Node.Name;

            if (string.IsNullOrEmpty(_title))
                _title = defaultTitle;

            SetTitle(_title);
        }

        /// <summary>
        /// Sets the color of the header of this node
        /// </summary>
        public void SetColor(float r, float g, float b) => HeaderBackground.style.backgroundColor = new Color(
            r / 255,
            g / 255,
            b / 255
        );

        /// <summary>
        /// Sets the tooltip of the header section
        /// </summary>
        public void SetTooltip(string newTooltip) => Header.tooltip = newTooltip ?? "";

        #endregion
    }
}