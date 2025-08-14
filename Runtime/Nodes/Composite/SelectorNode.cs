using TreeFlow.Core.Interfaces;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Composite
{
    /// <summary>
    /// Node that succeed when any of its children succeed. (OR)
    /// </summary>
    /// <remarks>
    /// If a child's state is <see cref="NodeStatus.RUNNING"/> or <see cref="NodeStatus.SUCCESS"/>,this node exits with this state.
    /// </remarks>
    public class SelectorNode : BaseComposite
    {
        public SelectorNode(params INode[] nodes) : base(nodes) { }

        /// <inheritdoc/>
        public override NodeStatus Evaluate()
        {
            foreach (var child in this)
            {
                var status = child.Evaluate();

                if (status == NodeStatus.RUNNING)
                    return NodeStatus.RUNNING;

                if (status == NodeStatus.SUCCESS)
                    return NodeStatus.SUCCESS;
            }

            return NodeStatus.FAILURE;
        }
    }
}