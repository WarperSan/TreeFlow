using TreeFlow.Runtime.Core;

namespace TreeFlow.Tests.Tests.Utils
{
    public static class NodeUtils
    {
        public static Node AlwaysFailure() => new AlwaysStatusNode(Node.NodeStatus.FAILURE);
        public static Node AlwaysSuccess() => new AlwaysStatusNode(Node.NodeStatus.SUCCESS);
        public static Node AlwaysRunning() => new AlwaysStatusNode(Node.NodeStatus.RUNNING);

        private class AlwaysStatusNode : Node
        {
            private readonly NodeStatus status;
            
            public AlwaysStatusNode(NodeStatus status)
            {
                this.status = status;
            }

            /// <inheritdoc/>
            public override NodeStatus Process() => status;
        }
    }
}