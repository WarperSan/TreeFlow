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

        private class StatusSequenceNode : Node
        {
            private readonly NodeStatus[] statuses;
            private int index = 0;
            
            public StatusSequenceNode(NodeStatus[] statuses)
            {
                this.statuses = statuses;
            }
            
            /// <inheritdoc/>
            public override NodeStatus Process()
            {
                var status = statuses[index];

                index++;
                
                if (index >= statuses.Length)
                    index = 0;

                return status;
            }
        }
    }
}