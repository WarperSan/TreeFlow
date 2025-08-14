using TreeFlow.Core.Interfaces;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Decorator
{
    /// <summary>
    /// Node that inverts the state of its child. (NOT)
    /// </summary>
    public class InverterNode : BaseDecorator
    {
        public InverterNode(INode child) : base(child)
        {
        }

        /// <inheritdoc/>
        protected override NodeStatus OnEvaluate()
        {
            var status = EvaluateChild();

            return status switch
            {
                NodeStatus.SUCCESS => NodeStatus.FAILURE,
                NodeStatus.FAILURE => NodeStatus.SUCCESS,
                _ => status
            };
        }

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override string Alias => "NOT";
#endif
    }
}