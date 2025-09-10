using TreeFlow.Editor.Helpers;
using TreeFlow.Editor.ScriptableObjects;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Resources = TreeFlow.Editor.Helpers.Resources;

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
            var targetTree = target as BehaviorTreeAsset;

            if (targetTree == null)
            {
                base.OnInspectorGUI();
                return;
            }
            
            if (GUILayout.Button("Open"))
                TreeFlowEditorWindow.Open(targetTree);
            
            GUILayout.Space(10);
            GUILayout.Label("Tools");

            if (GUILayout.Button("Remove Unused Nodes") && EditorUtility.DisplayDialog(
                    "Remove Unused Nodes",
                    "Are you sure you want to remove every unused nodes? This will remove any node that is not attached to the root.",
                    "Yes",
                    "No")
            ) {
                TreeJanitor.RemoveUnusedNodes(targetTree);
                Resources.SaveChanges(targetTree);
            }
        }
    }
}