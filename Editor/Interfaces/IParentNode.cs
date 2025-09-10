using System.Collections.Generic;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Interfaces
{
    /// <summary>
    /// Defines any <see cref="NodeAsset"/> that can own at least one child
    /// </summary>
    public interface IParentNode
    {
        /// <summary>
        /// Links the given node to this node
        /// </summary>
        public void Link(NodeAsset child);

        /// <summary>
        /// Unlinks the given node from this node
        /// </summary>
        public void Unlink(NodeAsset child);

        /// <summary>
        /// Unlinks every given node from this node
        /// </summary>
        public void Unlink(IEnumerable<NodeAsset> children)
        {
            var uniqueChildren = new HashSet<string>(Children);

            foreach (var child in children)
            {
                if (!uniqueChildren.Contains(child.GUID))
                    continue;
                
                Unlink(child);
            }
        }
        
        /// <summary>
        /// List of every child node of this node
        /// </summary>
        public IEnumerable<string> Children { get; }
    }
}