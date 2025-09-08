using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace TreeFlow.Editor.UIElements
{
    /// <summary>
    /// Element used to display and modify <see cref="BehaviorTreeAsset"/>
    /// </summary>
    internal class TreeGraphView : GraphView
    {
        public TreeGraphView()
        {
            CreateUI();
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDropper());
            this.AddManipulator(new RectangleSelector());
        }

        #region UI

        public new class UxmlFactory : UxmlFactory<TreeGraphView, UxmlTraits> { }

        private void CreateUI()
        {
            styleSheets.Add(Helpers.Resources.Load<StyleSheet>("StyleSheets/TreeGraphView.uss"));
            
            var background = new GridBackground();
            background.AddToClassList("grid");
            background.name = "Background";
            Add(background);
        }

        #endregion
    }
}