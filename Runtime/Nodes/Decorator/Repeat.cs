using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Decorator
{
    /// <summary>
    /// Node that allows only N failures from its child.
    /// </summary>
    /// <remarks>
    /// If the number of failures has been reached, the child will not be processed, and this node will return <see cref="Node.NodeStatus.FAILURE"/>.
    /// </remarks>
    public sealed class Repeat : Core.Decorator
    {
        private int remainingFails;

        public Repeat(int maxFails, Node child) : base(child)
        {
            if (maxFails < 0)
                maxFails = 0;
            
            remainingFails = maxFails;
        }

        /// <inheritdoc/>
        public override NodeStatus Process()
        {
            if (remainingFails <= 0)
                return NodeStatus.FAILURE;
            
            var status = Child.Process();

            if (status == NodeStatus.FAILURE)
                remainingFails--;

            return status;
        }
    }
}