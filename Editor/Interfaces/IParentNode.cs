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
    }
}