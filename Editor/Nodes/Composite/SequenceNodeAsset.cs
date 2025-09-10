using System;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Nodes.Composite
{
    [Serializable]
    public sealed class SequenceNodeAsset : CompositeNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);

            view.SetDefaultTitle("Sequence");
            view.SetColor(18, 152, 87);
        }
    }
}