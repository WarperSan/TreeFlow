using System;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Nodes.Leaf
{
    [Serializable]
    public class ConditionNodeAsset : LeafNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Condition");
            view.SetColor(44, 62, 80);
        }
    }
}