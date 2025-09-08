using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Editor that handles the link with the base client
    /// </summary>
    [CustomEditor(typeof(BehaviorTreeAsset))]
    public class TreeFlowEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Opens <see cref="TreeFlowEditorWindow"/> with the given asset
        /// </summary>
        private static void OpenWindow(BehaviorTreeAsset asset)
        {
            var window = EditorWindow.GetWindow<TreeFlowEditorWindow>();
            window.SetTree(asset);
            window.ShowTab();
        }
        
        [MenuItem("Window/TreeFlow/Designer")]
        private static void ShowWindow() => OpenWindow(null);

        [OnOpenAsset]
        private static bool OpenTree(int instanceID)
        {
            if (!AssetDatabase.CanOpenAssetInEditor(instanceID))
                return false;

            var path = AssetDatabase.GetAssetPath(instanceID);

            if (path == null)
                return false;

            var asset = AssetDatabase.LoadAssetAtPath<BehaviorTreeAsset>(path);

            if (asset is null)
                return false;

            OpenWindow(asset);
            return true;
        }
        
        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
                OpenWindow(target as BehaviorTreeAsset);
            
            //base.OnInspectorGUI();
        }
    }
}