using System;
using TreeFlow.Editor.Interfaces;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Leaf
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