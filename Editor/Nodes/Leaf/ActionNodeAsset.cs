using System;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Nodes.Leaf
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