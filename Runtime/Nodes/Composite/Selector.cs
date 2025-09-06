using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Composite
{
    /// <summary>
    /// Node that succeed when any of its children succeed. (OR)
    /// </summary>
    /// <remarks>
    /// If a child's state is <see cref="Node.NodeStatus.RUNNING"/> or <see cref="Node.NodeStatus.SUCCESS"/>,this node exits with this state.
    /// </remarks>
    public sealed class Selector : Core.Composite
    {
        public Selector(params Node[] nodes) : base(nodes) { }

        /// <inheritdoc/>
        public override NodeStatus Process()
        {
            foreach (var child in this)
            {
                var status = child.Process();

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (status == NodeStatus.RUNNING)
                    return NodeStatus.RUNNING;

                if (status == NodeStatus.SUCCESS)
                    return NodeStatus.SUCCESS;
            }

            return NodeStatus.FAILURE;
        }
    }
}