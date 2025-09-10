using System;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Nodes.Composite
{
    [Serializable]
    public sealed class ParallelNodeAsset : CompositeNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Parallel");
            view.SetColor(156, 162, 71);
        }
    }
}