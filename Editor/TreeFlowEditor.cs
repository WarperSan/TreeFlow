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
            FixChildNodesBtn(targetTree);
            OptimizeTreeBtn(targetTree);

            if (EditorUtility.IsDirty(targetTree))
            {
                GUI.enabled = true;
                EditorGUILayout.HelpBox($"'{targetTree.name}' has some unsaved changes.", MessageType.Warning);
            }
            
            base.OnInspectorGUI();
        }

        private static void RemoveUnusedNodesBtn(BehaviorTreeAsset tree)
        {
            if (!GUILayout.Button(new GUIContent("Remove Unused Nodes", "Removes any node that isn't connected to the tree")))
                return;
            
            if (!EditorUtility.DisplayDialog("Confirmation - Remove Unused Nodes", "Are you sure you want to proceed? This alters the tree itself.", "Yes", "No"))
                return;
            
            tree.Compute();
            TreeSanitizer.RemoveDetachedNodes(tree);
            Resources.SaveChanges(tree);
            TreeFlowEditorWindow.Open(tree);
        }

        private static void FixChildNodesBtn(BehaviorTreeAsset tree)
        {
            if (!GUILayout.Button(new GUIContent("Fix Child Nodes", "Fixes the child references in the tree")))
                return;
            
            if (!EditorUtility.DisplayDialog("Confirmation - Fix Child Nodes", "Are you sure you want to proceed? This alters the tree itself.", "Yes", "No"))
                return;
            
            tree.Compute();
            TreeSanitizer.FixChildNodes(tree);
            Resources.SaveChanges(tree);
            TreeFlowEditorWindow.Open(tree);
        }

        private static void OptimizeTreeBtn(BehaviorTreeAsset tree)
        {
            if (!GUILayout.Button(new GUIContent("Optimize Tree", "Optimizes the tree based of known rules")))
                return;
            
            if (!EditorUtility.DisplayDialog("Confirmation - Optimize Tree", "Are you sure you want to proceed? This alters the tree itself.", "Yes", "No"))
                return;
            
            tree.Compute();
            TreeSanitizer.FixChildNodes(tree);
            TreeSanitizer.RemoveDetachedNodes(tree);
            TreeOptimizer.Optimize(tree);
            Resources.SaveChanges(tree);
            TreeFlowEditorWindow.Open(tree);
        }

        #endregion
    }
}