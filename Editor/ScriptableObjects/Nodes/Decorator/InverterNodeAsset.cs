using System;
using TreeFlow.Editor.Interfaces;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Decorator
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