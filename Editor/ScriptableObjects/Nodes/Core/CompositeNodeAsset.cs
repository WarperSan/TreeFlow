using System;
using TreeFlow.Editor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace TreeFlow.Editor.ScriptableObjects
{
    [Serializable]
    public abstract class CompositeNodeAsset : NodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(NodeView view)
        {
            base.Customize(view);
            
            view.inputContainer.Add(view.InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool)));
            view.outputContainer.Add(view.InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool)));
        }
    }
}