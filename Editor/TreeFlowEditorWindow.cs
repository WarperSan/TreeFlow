using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Editor window that allows to see and design <see cref="BehaviorTreeAsset"/>
    /// </summary>
    public class TreeFlowEditorWindow : EditorWindow
    {
        private void CreateGUI() => CreateUI();

        /// <summary>
        /// Sets the tree in used to the given tree
        /// </summary>
        public void SetTree(BehaviorTreeAsset tree)
        {
            SetTitle(tree?.name);
        }

        #region Window

        /// <summary>
        /// Sets the title of this window to the given title
        /// </summary>
        private void SetTitle(string newTitle)
        {
            titleContent.text = newTitle ?? "Behavior Tree";
        }

        #endregion

        #region UI

        [SerializeField] private VisualTreeAsset editorWindowUXML;

        private void CreateUI()
        {
            editorWindowUXML.CloneTree(rootVisualElement);
        }

        #endregion
    }
}