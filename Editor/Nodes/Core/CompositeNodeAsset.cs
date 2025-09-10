using System;
using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.ScriptableObjects;
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
        public void Link(NodeAsset child)
        {
            m_children.Add(child.GUID);
            uniqueChildren.Add(child.GUID);
        }

        /// <inheritdoc/>
        public void Unlink(NodeAsset child)
        {
            m_children.Remove(child.GUID);
            uniqueChildren.Remove(child.GUID);
        }

        #region Utils

        [NonSerialized] private readonly HashSet<string> uniqueChildren = new();

        /// <inheritdoc/>
        public override void Compute(BehaviorTreeAsset tree)
        {
            base.Compute(tree);
            
            uniqueChildren.Clear();
            
            foreach (var child in m_children)
                uniqueChildren.Add(child);
        }

        #endregion
    }
}