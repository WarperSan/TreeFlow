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
        private BehaviorTreeAsset treeAsset;
        private TreeGraphView treeGraphView;
        
        #region EditorWindow

        /// <summary>
        /// Opens this window with the given asset
        /// </summary>
        public static void Open(BehaviorTreeAsset asset = null)
        {
            var window = GetWindow<TreeFlowEditorWindow>();
            window.SetTree(asset);
            window.ShowTab();
        }

        private void OnEnable()
        {
            if (treeAsset is null)
                return;

            SetTree(treeAsset);
        }
        
        private void OnGUI()
        {
            if (treeAsset is null)
                return;

            hasUnsavedChanges = EditorUtility.IsDirty(treeAsset);
        }
        
        private void CreateGUI()
        {
            var editorWindowUXML = Helpers.Resources.Load<VisualTreeAsset>(Helpers.Resources.EDITOR_WINDOW_UXML);
            editorWindowUXML.CloneTree(rootVisualElement);

            treeGraphView = rootVisualElement.Q<TreeGraphView>();
            treeGraphView.OnTreeChanged += OnTreeChanged;
            
            SetTree(treeAsset);
        }

        #endregion

        #region Tree

        /// <summary>
        /// Sets the tree in used to the given tree
        /// </summary>
        private void SetTree(BehaviorTreeAsset tree)
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

            treeGraphView?.PopulateView(tree);
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
            foreach (var node in treeAsset.Nodes)
                Helpers.Resources.DiscardChanges(node);

            Helpers.Resources.DiscardChanges(treeAsset);
            base.DiscardChanges();
        }

        /// <inheritdoc/>
        public override void SaveChanges()
        {
            Helpers.Resources.SaveChanges(treeAsset);
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