namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node in a tree
    /// </summary>
    public abstract class BaseNode
    {
        #region Evaluation

#if UNITY_EDITOR
        /// <summary>
        /// Current status of this node
        /// </summary>
        internal NodeStatus Status { get; private set; }

        /// <summary>
        /// Resets the status of this node
        /// </summary>
        internal virtual void Reset()
        {
            Status = NodeStatus.NONE;
        }
#endif
        
        /// <summary>
        /// Evaluates this node
        /// </summary>
        /// <returns>Status of this node</returns>
        public abstract NodeStatus Evaluate();

        #endregion

        #region Hierarchy
        
        private BaseNode parent;

        /// <summary>
        /// Sets the parent of this node
        /// </summary>
        /// <param name="node">New parent of this node</param>
        public void SetParent(BaseNode node) => parent = node;

        #endregion
    }
}