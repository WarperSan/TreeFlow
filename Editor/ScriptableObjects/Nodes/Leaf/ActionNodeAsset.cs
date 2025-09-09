using System;
using TreeFlow.Editor.UIElements;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Leaf
{
    [Serializable]
    public class ActionNodeAsset : LeafNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(NodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Action");
            view.SetColor(142, 50, 172);
        }
    }
}