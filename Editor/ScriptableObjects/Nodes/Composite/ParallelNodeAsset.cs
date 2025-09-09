using System;
using TreeFlow.Editor.UIElements;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Composite
{
    [Serializable]
    public sealed class ParallelNodeAsset : CompositeNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(NodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Parallel");
            view.SetColor(156, 162, 71);
        }
    }
}