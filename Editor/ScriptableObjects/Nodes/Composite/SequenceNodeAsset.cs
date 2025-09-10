using System;
using TreeFlow.Editor.Interfaces;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Composite
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