using TreeFlow.Core.Interfaces;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Composite
{
    /// <summary>
    /// Node that only succeed when all of its children succeed. (AND)
    /// </summary>
    /// <remarks>
    /// If a child's state is <see cref="NodeStatus.RUNNING"/> or <see cref="NodeStatus.FAILURE"/>,this node exits with this state.
    /// </remarks>
    public class SequenceNode : BaseComposite
    {
        public SequenceNode(params INode[] nodes) : base(nodes) { }

        /// <inheritdoc/>
        public override NodeStatus Evaluate()
        {
            foreach (var child in this)
            {
                var status = child.Evaluate();

                if (status == NodeStatus.RUNNING)
                    return NodeStatus.RUNNING;

                if (status == NodeStatus.FAILURE)
                    return NodeStatus.FAILURE;
            }

            return NodeStatus.SUCCESS;
        }
    }
}