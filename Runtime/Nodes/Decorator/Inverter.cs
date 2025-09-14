using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Decorator
{
    /// <summary>
    ///     Node that inverts the state of its child. (NOT)
    /// </summary>
    public sealed class Inverter : Core.Decorator
    {
        public Inverter(Node child) : base(child) { }

        public override NodeStatus Process()
        {
            var status = Child.Process();

            return status switch
            {
                NodeStatus.SUCCESS => NodeStatus.FAILURE,
                NodeStatus.FAILURE => NodeStatus.SUCCESS,
                _ => status
            };
        }
    }
}