namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing only one child
    /// </summary>
    public abstract class BaseDecorator : BaseNode
    {
        protected BaseDecorator(BaseNode child)
        {
            this.child = child;
        }

        #region Hierarchy

        private readonly BaseNode child;
        
#if UNITY_EDITOR
        /// <summary>
        /// Gets the child of this node
        /// </summary>
        internal BaseNode GetChild() => child;
        
        /// <inheritdoc/>
        internal override void Reset()
        {
            base.Reset();
            child.Reset();
        }
#endif

        protected NodeStatus EvaluateChild() => child.Evaluate();

        #endregion
    }
}