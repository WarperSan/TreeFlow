using TreeFlow.Editor.Nodes.Composite;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.Nodes.Decorator;
using TreeFlow.Editor.Nodes.Leaf;
using TreeFlow.Runtime.Core;
using TreeFlow.Runtime.Nodes.Composite;
using TreeFlow.Runtime.Nodes.Decorator;
using TreeFlow.Runtime.Nodes.Leaf;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    ///     Class that handles the transition from <see cref="Node" /> to <see cref="NodeAsset" />
    /// </summary>
    internal static class NodeRegistry
    {
        /// <summary>
        ///     Creates an instance of <see cref="NodeAsset" /> that represents the given node type
        /// </summary>
        /// <remarks>
        ///     If the given type isn't handled, <c>null</c> will be returned
        /// </remarks>
        public static NodeAsset CreateAsset<T>()
        {
            var type = typeof(T);
            
            if (typeof(NodeAsset).IsAssignableFrom(type))
                return (NodeAsset)System.Activator.CreateInstance(type);
            
            if (!typeof(Node).IsAssignableFrom(type))
                return null;

            if (type == typeof(Selector))
                return new SelectorNodeAsset();
            
            if (type == typeof(Sequence))
                return new SequenceNodeAsset();
            
            if (type == typeof(Parallel))
                return new ParallelNodeAsset();

            if (type == typeof(Inverter))
                return new InverterNodeAsset();

            if (type == typeof(Condition))
                return new ConditionNodeAsset();
            
            if (type == typeof(Action))
                return new ActionNodeAsset();

            return null;
        }
    }
}