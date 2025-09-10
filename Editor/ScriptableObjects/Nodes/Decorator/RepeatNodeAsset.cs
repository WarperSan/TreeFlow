using System;
using TreeFlow.Editor.Interfaces;

namespace TreeFlow.Editor.ScriptableObjects.Nodes.Decorator
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