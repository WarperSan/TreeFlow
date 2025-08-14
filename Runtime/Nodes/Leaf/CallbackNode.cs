using System;
using TreeFlow.Core.Interfaces;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Leaf
{
    /// <summary>
    /// Node that executes the given action
    /// </summary>
    public class CallbackNode : BaseNode
    {
        private readonly Func<INode, NodeStatus> CallBack;

        public CallbackNode(Func<INode, NodeStatus> callback)
        {
            CallBack = callback;
        }

        #region BaseNode

        /// <inheritdoc/>
        public override NodeStatus Evaluate() => CallBack(this);

        #endregion
    }
}