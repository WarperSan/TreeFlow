using System.Collections.Generic;
using TreeFlow.Core.Interfaces;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing only one child
    /// </summary>
    public abstract class BaseDecorator : BaseNode, IParentNode
    {
        protected BaseDecorator(BaseNode child)
        {
            this.child = child;
        }

        #region Hierarchy

        private readonly BaseNode child;
        
#if UNITY_EDITOR
        /// <inheritdoc/>
        internal override void Reset()
        {
            base.Reset();
            child.Reset();
        }
#endif

        protected NodeStatus EvaluateChild() => child.Evaluate();

        #endregion

        #region IParentNode

        /// <inheritdoc/>
        IEnumerable<BaseNode> IParentNode.GetChildren() => new []{ child };

        #endregion
    }
}