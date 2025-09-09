using TreeFlow.Editor.ScriptableObjects;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Editor that handles the link with the base client
    /// </summary>
    [CustomEditor(typeof(BehaviorTreeAsset))]
    internal class TreeFlowEditor : UnityEditor.Editor
    {
        [MenuItem("Window/TreeFlow/Designer")]
        private static void ShowWindow() => TreeFlowEditorWindow.Open();

        [OnOpenAsset]
        private static bool OpenTree(int instanceID)
        {
            if (!AssetDatabase.CanOpenAssetInEditor(instanceID))
                return false;

            var path = AssetDatabase.GetAssetPath(instanceID);

            if (string.IsNullOrEmpty(path))
                return false;

            var asset = AssetDatabase.LoadAssetAtPath<BehaviorTreeAsset>(path);

            if (asset is null)
                return false;

            TreeFlowEditorWindow.Open(asset);
            return true;
        }
        
        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
                TreeFlowEditorWindow.Open(target as BehaviorTreeAsset);
        }
    }
}