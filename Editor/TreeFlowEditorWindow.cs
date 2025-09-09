using TreeFlow.Editor.ScriptableObjects;
using TreeFlow.Editor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Editor window that allows to see and design <see cref="BehaviorTreeAsset"/>
    /// </summary>
    internal class TreeFlowEditorWindow : EditorWindow
    {
        #region EditorWindow

        private void OnEnable() => SetTree(treeAsset);

        #endregion

        #region UI

        private TreeGraphView treeGraphView;

        private void CreateGUI()
        {
            var editorWindowUXML = Helpers.Resources.Load<VisualTreeAsset>(Helpers.Resources.EDITOR_WINDOW_UXML);
            editorWindowUXML.CloneTree(rootVisualElement);

            treeGraphView = rootVisualElement.Q<TreeGraphView>();

            treeGraphView.OnTreeChanged += OnTreeChanged;
            
            SetTree(treeAsset);
        }
        
        private void OnGUI()
        {
            hasUnsavedChanges = EditorUtility.IsDirty(treeAsset);
        }

        #endregion

        #region Tree

        private BehaviorTreeAsset treeAsset;

        /// <summary>
        /// Sets the tree in used to the given tree
        /// </summary>
        public void SetTree(BehaviorTreeAsset tree)
        {
            if (treeAsset is not null && hasUnsavedChanges && PromptChanges())
                return;
            
            if (tree == null)
            {
                titleContent.text = "Behavior Tree";
                treeAsset = null;
                return;
            }

            titleContent.text = tree.name;
            treeAsset = tree;

            if (treeGraphView is null)
                return;

            var obj = new SerializedObject(tree);
            treeGraphView.AssignTree(obj);
        }

        #endregion

        #region Changes

        /// <summary>
        /// Called when <see cref="treeGraphView"/> is changed
        /// </summary>
        private void OnTreeChanged()
        {
            hasUnsavedChanges = true;
            EditorUtility.SetDirty(treeAsset);
        }

        /// <inheritdoc/>
        public override void DiscardChanges()
        {
            if (treeAsset is not null)
                Resources.UnloadAsset(treeAsset);
            
            base.DiscardChanges();
        }

        /// <inheritdoc/>
        public override void SaveChanges()
        {
            if (EditorUtility.IsDirty(treeAsset))
            {
                AssetDatabase.SaveAssetIfDirty(treeAsset);
                AssetDatabase.Refresh();
            }
            
            base.SaveChanges();
        }

        /// <summary>
        /// Prompts the user to react to the changes
        /// </summary>
        /// <returns>Prompt cancelled</returns>
        private bool PromptChanges()
        {
            var optionIndex = EditorUtility.DisplayDialogComplex(
                "Tree Has Been Modified",
                "Do you want to save the changes you made to the tree? Your changes will be lost if you don't save them.",
                "Save",
                "Cancel",
                "Discard Changes"
            );
                
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (optionIndex == 1)
                return true;

            if (optionIndex == 0)
                SaveChanges();
            else
                DiscardChanges();
            return false;
        }
        
        #endregion
    }
}