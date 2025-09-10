using System;
using TreeFlow.Editor.Interfaces;
using UnityEngine;

namespace TreeFlow.Editor.Nodes.Core
{
    [Serializable]
    public abstract class DecoratorNodeAsset : NodeAsset, IParentNode
    {
        /// <summary>
        /// <see cref="NodeAsset.GUID"/> of the child of this node
        /// </summary>
        [SerializeField] private string m_child;

        /// <inheritdoc/>
        public void Link(NodeAsset child)
        {
            this.m_child = child.GUID;
        }

        /// <inheritdoc/>
        public void Unlink(NodeAsset child)
        {
            if (this.m_child != child.GUID)
                return;

            this.m_child = null;
        }
    }
}