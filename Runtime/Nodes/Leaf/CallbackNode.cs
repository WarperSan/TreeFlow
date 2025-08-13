using System;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Leaf
{
    /// <summary>
    /// Node that executes the given action
    /// </summary>
    public class CallbackNode : BaseNode
    {
        private readonly Func<BaseNode, NodeStatus> CallBack;

        public CallbackNode(Func<BaseNode, NodeStatus> callback)
        {
            CallBack = callback;
        }

        #region BaseNode

        /// <inheritdoc/>
        public override NodeStatus Evaluate() => CallBack(this);

        #endregion
    }
}