using System;

namespace TreeFlow.Editor.Nodes.Core
{
    [Serializable]
    public abstract class DecoratorNodeAsset : NodeAsset
    {
        /// <summary>
        /// <see cref="NodeAsset.GUID"/> of the child of this node
        /// </summary>
        public string Child;
    }
}