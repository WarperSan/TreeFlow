using System;
using TreeFlow.Editor.Interfaces;

namespace TreeFlow.Editor.Nodes.Core
{
    [Serializable]
    public abstract class CompositeNodeAsset : NodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);
            
            view.AddInputPort();
            view.AddOutputPort(true);
        }
    }
}