using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Composite
{
    /// <summary>
    ///     Node that processes all of its children.
    /// </summary>
    /// <remarks>
    ///     If enough children have the same state, this node exits with this state.
    /// </remarks>
    public sealed class Parallel : Core.Composite
    {
        private readonly int threshold;

        public Parallel(int threshold, params Node[] nodes) : base(nodes)
        {
            if (threshold < 0)
                threshold = 0;

            this.threshold = threshold;
        }

        /// <inheritdoc />
        public override NodeStatus Process()
        {
            var totalCount = 0;
            var successCount = 0;
            var failureCount = 0;

            foreach (var child in this)
            {
                var status = child.Process();

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (status == NodeStatus.FAILURE)
                    failureCount++;
                else if (status == NodeStatus.SUCCESS)
                    successCount++;

                totalCount++;
            }

            if (successCount >= threshold)
                return NodeStatus.SUCCESS;

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (failureCount > totalCount - threshold)
                return NodeStatus.FAILURE;

            return NodeStatus.RUNNING;
        }
    }
}