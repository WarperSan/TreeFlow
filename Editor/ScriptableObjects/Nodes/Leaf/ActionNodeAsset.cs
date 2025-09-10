using System;
using TreeFlow.Editor.Interfaces;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Leaf
{
    [Serializable]
    public class ActionNodeAsset : LeafNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Action");
            view.SetColor(182, 59, 47);
        }
    }
}