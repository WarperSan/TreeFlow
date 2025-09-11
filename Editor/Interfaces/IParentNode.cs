using System.Collections.Generic;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.ScriptableObjects;

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
            foreach (var child in children)
                Unlink(child);
        }

        /// <summary>
        /// Replaces <see cref="oldChild"/> with <see cref="newChild"/>
        /// </summary>
        public void Replace(NodeAsset oldChild, NodeAsset newChild);
        
        /// <summary>
        /// List of every child node of this node
        /// </summary>
        public IEnumerable<string> Children { get; }

        /// <summary>
        /// List of every node of this node
        /// </summary>
        public IEnumerable<NodeAsset> GetChildren(BehaviorTreeAsset tree)
        {
            var assets = new NodeAsset[Count];
            var index = 0;

            foreach (var child in Children)
            {
                assets[index] = tree.GetNode(child);
                index++;
            }

            return assets;
        }
        
        /// <summary>
        /// Amount of children this node has
        /// </summary>
        public int Count { get; }
    }
}