using TreeFlow.Editor.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

        private void OnGUI() => CheckDirty();


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
        private TreeGraphView treeGraphView;

        private void CreateUI()
        {
            editorWindowUXML.CloneTree(rootVisualElement);

            treeGraphView = rootVisualElement.Q<TreeGraphView>();
            treeGraphView.graphViewChanged += OnGraphChanged;
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
            
            SetTitle(tree?.name);

            var obj = new SerializedObject(tree);
            treeGraphView?.AssignTree(obj);
            
            treeAsset = tree;
        }

        #endregion

        #region Dirty

        /// <summary>
        /// Called when <see cref="treeGraphView"/> is changed
        /// </summary>
        private GraphViewChange OnGraphChanged(GraphViewChange graphViewChange)
        {
            hasUnsavedChanges = true;
            EditorUtility.SetDirty(treeAsset);
            return graphViewChange;
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