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
        
        #region Inspector
        
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

            if (EditorUtility.IsDirty(targetTree))
                GUI.enabled = false;

            RemoveUnusedNodesBtn(targetTree);
            OrderChildNodesBtn(targetTree);

            if (EditorUtility.IsDirty(targetTree))
            {
                GUI.enabled = true;
                EditorGUILayout.HelpBox($"'{targetTree.name}' has some unsaved changes.", MessageType.Warning);
            }
        }

        private static void RemoveUnusedNodesBtn(BehaviorTreeAsset tree)
        {
            if (!GUILayout.Button(new GUIContent("Remove Unused Nodes", "Removes any node that isn't connected to the tree")))
                return;
            
            if (!EditorUtility.DisplayDialog("Confirmation - Remove Unused Nodes", "Are you sure you want to proceed? This alters the tree itself.", "Yes", "No"))
                return;
            
            TreeJanitor.RemoveUnusedNodes(tree);
            Resources.SaveChanges(tree);
        }
        private static void OrderChildNodesBtn(BehaviorTreeAsset tree)
        {
            if (!GUILayout.Button(new GUIContent("Order Child Nodes", "Orders the child references in the tree")))
                return;
            
            if (!EditorUtility.DisplayDialog("Confirmation - Order Child Nodes", "Are you sure you want to proceed? This alters the tree itself.", "Yes", "No"))
                return;
            
            TreeJanitor.OrderChildNodes(tree);
            Resources.SaveChanges(tree);
        }

        #endregion
    }
}