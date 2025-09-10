using System;
using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using UnityEngine;

namespace TreeFlow.Editor.Nodes.Core
{
    [Serializable]
    public abstract class CompositeNodeAsset : NodeAsset, IParentNode
    {
        /// <summary>
        /// List of children of this node
        /// </summary>
        [SerializeField] private List<string> m_children = new();
        
        /// <inheritdoc/>
        public void Link(NodeAsset child) => m_children.Add(child.GUID);

        /// <inheritdoc/>
        public void Unlink(NodeAsset child) => m_children.Remove(child.GUID);

        /// <inheritdoc/>
        public IEnumerable<string> Children => m_children;
    }
}