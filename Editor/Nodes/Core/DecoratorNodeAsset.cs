using System;
using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.ScriptableObjects;
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
            m_child = child.GUID;
        }

        /// <inheritdoc/>
        public void Unlink(NodeAsset child)
        {
            if (m_child != child.GUID)
                return;

            m_child = null;
        }

        /// <inheritdoc/>
        public void Replace(NodeAsset oldChild, NodeAsset newChild)
        {
            if (m_child != oldChild.GUID)
                return;
            
            m_child = newChild.GUID;
        }

        /// <inheritdoc/>
        public IEnumerable<string> Children => new[] { m_child };
        
        /// <summary>
        /// Gets the child of this node
        /// </summary>
        public NodeAsset GetChild(BehaviorTreeAsset tree) => tree.GetNode(m_child);

        /// <inheritdoc/>
        public int Count => m_child != null ? 1 : 0;
    }
}