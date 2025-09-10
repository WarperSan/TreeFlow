using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor.UIElements
{
    /// <summary>
    /// Element used to display and modify <see cref="NodeAsset"/>
    /// </summary>
    public sealed class NodeView : Node, INodeView
    {
        /// <summary>
        /// Current node displayed
        /// </summary>
        public readonly NodeAsset Node;
        
        private readonly TreeGraphView graphView;
        
        internal NodeView(NodeAsset node, TreeGraphView graphView) : base(Helpers.Resources.RelativeToAbsolute(Helpers.Resources.NODE_VIEW_UXML))
        {
            styleSheets.Add(Helpers.Resources.Load<StyleSheet>(Helpers.Resources.NODE_VIEW_USS));
            
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

        #endregion

        #region INodeView
        
        /// <inheritdoc/>
        public void SetTitle(string newTitle) => title = TitleInput.value = newTitle;

        /// <inheritdoc/>
        public void SetDefaultTitle(string defaultTitle)
        {
            var _title = Node.Name;

            if (string.IsNullOrEmpty(_title))
                _title = defaultTitle;

            SetTitle(_title);
        }

        /// <inheritdoc/>
        public void SetColor(byte r, byte g, byte b) => HeaderBackground.style.backgroundColor = new Color(
            r / 255f,
            g / 255f,
            b / 255f
        );

        /// <inheritdoc/>
        public void AddInputPort()
        {
            if (inputContainer is null)
                return;

            var input = InstantiatePort(
                Orientation.Vertical,
                Direction.Input,
                Port.Capacity.Single,
                typeof(bool)
            );
            inputContainer.Add(input);
        }

        /// <inheritdoc/>
        public void AddOutputPort(bool allowMultiple = false)
        {
            if (outputContainer is null)
                return;

            var output = InstantiatePort(
                Orientation.Vertical,
                Direction.Output,
                allowMultiple ? Port.Capacity.Multi : Port.Capacity.Single,
                typeof(bool)
            );
            outputContainer.Add(output);
        }

        #endregion
    }
}