using System;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Nodes.Decorator
{
    [Serializable]
    public class RepeatNodeAsset : DecoratorNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Repeat");
            view.SetColor(22, 160, 133);
        }
    }
}