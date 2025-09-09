using System;
using TreeFlow.Editor.UIElements;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Decorator
{
    [Serializable]
    public class InverterNodeAsset : DecoratorNodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(NodeView view)
        {
            base.Customize(view);
            
            view.SetDefaultTitle("Inverter");
            view.SetColor(155, 89, 182);
        }
    }
}