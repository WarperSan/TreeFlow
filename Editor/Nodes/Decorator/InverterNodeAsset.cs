using System;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Nodes.Decorator
{
    [Serializable]
    public class InverterNodeAsset : DecoratorNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(INodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Inverter");
            view.SetColor(155, 89, 182);
        }
    }
}