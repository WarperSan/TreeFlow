using TreeFlow.Editor.Elements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Window used to edit trees inside the editor
    /// </summary>
    internal class TreeEditorWindow : EditorWindow
    {
        [MenuItem("Window/TreeFlow/Tree Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<TreeEditorWindow>();
            window.titleContent = new GUIContent("Tree Editor");
        }

        private void OnEnable()
        {
            var graphView = new TreeView
            {
                name = "Behavior Tree Graph"
            };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
    }
}