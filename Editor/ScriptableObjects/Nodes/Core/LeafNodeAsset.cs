using System;
using TreeFlow.Editor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace TreeFlow.Editor.ScriptableObjects
{
    [Serializable]
    public abstract class LeafNodeAsset : NodeAsset
    {
        /// <inheritdoc/>
        public override void Customize(NodeView view)
        {
            base.Customize(view);
            
            view.inputContainer.Add(view.InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool)));
        }
    }
}