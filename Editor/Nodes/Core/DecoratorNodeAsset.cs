using System;
using UnityEngine;

namespace TreeFlow.Editor.Nodes.Core
{
    [Serializable]
    public abstract class DecoratorNodeAsset : NodeAsset
    {
        /// <summary>
        /// <see cref="NodeAsset.GUID"/> of the child of this node
        /// </summary>
        [SerializeField] private string Child;

        /// <summary>
        /// Replaces the current child with the given one
        /// </summary>
        public void ReplaceChild(NodeAsset child)
        {
            Child = child.GUID;
        }
    }
}