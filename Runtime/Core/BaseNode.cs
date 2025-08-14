using TreeFlow.Core.Interfaces;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node in a tree
    /// </summary>
    public abstract class BaseNode : INode
    {
        #region Evaluation

        /// <summary>
        /// Evaluates this node
        /// </summary>
        /// <returns>Status of this node</returns>
        public abstract NodeStatus Evaluate();

        #endregion

        #region Hierarchy
        
        private INode parent;

        /// <inheritdoc/>
        public void SetParent(INode node) => parent = node;

        #endregion
    }
}