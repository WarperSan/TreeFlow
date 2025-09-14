using System;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Leaf
{
    /// <summary>
    ///     Node that evaluates the given proposition
    /// </summary>
    /// <remarks>
    ///     If the proposition holds, <see cref="Node.NodeStatus.SUCCESS" /> is returned. Otherwise,
    ///     <see cref="Node.NodeStatus.FAILURE" />.
    /// </remarks>
    public sealed class Condition : Core.Leaf
    {
        private readonly Predicate<Node> Proposition;

        public Condition(Predicate<Node> proposition)
        {
            Proposition = proposition;
        }

        /// <inheritdoc />
        public override NodeStatus Process()
        {
            if (Proposition == null)
                return NodeStatus.FAILURE;

            return Proposition.Invoke(this) ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }
}