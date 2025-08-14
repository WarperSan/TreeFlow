using TreeFlow.Runtime.Core;

namespace TreeFlow.Core.Interfaces
{
    /// <summary>
    /// Defines every class that can act as a node in a <see cref="BaseTree"/>
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Evaluates this node
        /// </summary>
        /// <returns>Status of this node</returns>
        public NodeStatus Evaluate();
        
        /// <summary>
        /// Sets the parent of this node
        /// </summary>
        /// <param name="node">New parent of this node</param>
        public void SetParent(INode node);
    }
}