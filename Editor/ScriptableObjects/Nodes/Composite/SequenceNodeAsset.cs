using System;
using TreeFlow.Editor.UIElements;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Composite
{
    [Serializable]
    public sealed class SequenceNodeAsset : CompositeNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(NodeView view)
        {
            base.Customize(view);

            view.SetDefaultTitle("Sequence");
            view.SetColor(18, 152, 87);
        }
    }
}