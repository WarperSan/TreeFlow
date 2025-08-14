using System.Collections.Generic;
using TreeFlow.Runtime.Core.Interfaces;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing only one child
    /// </summary>
    public abstract class BaseDecorator : BaseNode, IParentNode
    {
        protected BaseDecorator(INode child)
        {
            this.child = child;
        }

        #region Hierarchy

        private readonly INode child;

        protected NodeStatus EvaluateChild() => child.Evaluate();

        #endregion

        #region IParentNode

        /// <inheritdoc/>
        IEnumerable<INode> IParentNode.GetChildren() => new []{ child };

        #endregion
    }
}