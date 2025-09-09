using TreeFlow.Editor.UIElements;
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
        #region Window
        
        private void CreateGUI() => CreateUI();

        private void OnGUI() => CheckDirty();

        private void OnEnable() => SetTree(treeAsset);

        #endregion

        #region UI

        [SerializeField] private VisualTreeAsset editorWindowUXML;
        private TreeGraphView treeGraphView;

        private void CreateUI()
        {
            editorWindowUXML.CloneTree(rootVisualElement);

            treeGraphView = rootVisualElement.Q<TreeGraphView>();

            treeGraphView.OnTreeChanged += OnTreeChanged;
            
            if (treeAsset is null)
                return;

            var obj = new SerializedObject(treeAsset);
            treeGraphView.AssignTree(obj);
        }

        #endregion

        #region Tree

        private BehaviorTreeAsset treeAsset;

        /// <summary>
        /// Sets the tree in used to the given tree
        /// </summary>
        public void SetTree(BehaviorTreeAsset tree)
        {
            if (treeAsset is not null && hasUnsavedChanges)
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
                    return;

                if (optionIndex == 0)
                    SaveChanges();
                else
                    DiscardChanges();
            }
            
            if (tree is null)
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

        #region Dirty

        /// <summary>
        /// Called when <see cref="treeGraphView"/> is changed
        /// </summary>
        private void OnTreeChanged()
        {
            hasUnsavedChanges = true;
            EditorUtility.SetDirty(treeAsset);
        }

        /// <summary>
        /// Checks if <see cref="treeAsset"/> is dirty, and saves it if necessary
        /// </summary>
        private void CheckDirty()
        {
            if (treeAsset is null)
                return;
            
            hasUnsavedChanges = EditorUtility.IsDirty(treeAsset);
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
            if (treeAsset != null && EditorUtility.IsDirty(treeAsset))
            {
                AssetDatabase.SaveAssetIfDirty(treeAsset);
                AssetDatabase.Refresh();
            }
            
            base.SaveChanges();
        }
        
        #endregion
    }
}