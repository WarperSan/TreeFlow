using System;
using TreeFlow.Editor.Interfaces;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Composite
{
    [Serializable]
    public sealed class SelectorNodeAsset : CompositeNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Selector");
            view.SetColor(17, 93, 168);
        }
    }
}