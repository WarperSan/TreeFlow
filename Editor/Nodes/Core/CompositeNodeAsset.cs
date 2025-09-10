using System;
using System.Collections.Generic;
using TreeFlow.Editor.ScriptableObjects;

namespace TreeFlow.Editor.Nodes.Core
{
    [Serializable]
    public abstract class CompositeNodeAsset : NodeAsset
    {
        /// <summary>
        /// List of children of this node
        /// </summary>
        public List<string> Children;

        /// <summary>
        /// Links the given node to this node
        /// </summary>
        public void Link(string guid)
        {
            Children.Add(guid);
            uniqueChildren.Add(guid);
        }

        /// <summary>
        /// Unlinks the given node from this node
        /// </summary>
        public void Unlink(string guid)
        {
            Children.Remove(guid);
            uniqueChildren.Remove(guid);
        }

        #region Utils

        [NonSerialized] private readonly HashSet<string> uniqueChildren = new();

        /// <inheritdoc/>
        public override void Compute(BehaviorTreeAsset tree)
        {
            base.Compute(tree);
            
            uniqueChildren.Clear();
            
            foreach (var child in Children)
                uniqueChildren.Add(child);
        }

        #endregion
    }
}