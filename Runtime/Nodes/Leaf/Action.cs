using System;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Runtime.Nodes.Leaf
{
    // TODO: Make the command async to handle RUNNING
    // SUCCESS => Command succeeded
    // FAILURE => Command failed
    // RUNNING => Command running

    /// <summary>
    ///     Node that executes the given command
    /// </summary>
    public sealed class Action : Core.Leaf
    {
        private readonly Func<Node, NodeStatus> Command;

        public Action(Func<Node, NodeStatus> command)
        {
            Command = command;
        }

        /// <inheritdoc />
        public override NodeStatus Process() => Command?.Invoke(this) ?? NodeStatus.FAILURE;
    }
}