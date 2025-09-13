using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Composite
{
    /// <summary>
    ///     Node that only succeed when all of its children succeed. (AND)
    /// </summary>
    /// <remarks>
    ///     If a child's state is <see cref="NodeStatus.RUNNING" /> or <see cref="NodeStatus.FAILURE" />,this node exits with
    ///     this state.
    /// </remarks>
    public sealed class Sequence : Core.Composite
    {
        public Sequence(params Node[] nodes) : base(nodes) { }

        /// <inheritdoc />
        public override NodeStatus Process()
        {
            foreach (var child in this)
            {
                var status = child.Process();

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (status == NodeStatus.RUNNING)
                    return NodeStatus.RUNNING;

                if (status == NodeStatus.FAILURE)
                    return NodeStatus.FAILURE;
            }

            return NodeStatus.SUCCESS;
        }
    }
}