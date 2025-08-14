using TreeFlow.Runtime.Core;

namespace TreeFlow.Core.Interfaces
{
    /// <summary>
    /// Defines every class that can act as a node in a <see cref="BaseTree"/>
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Evaluates this node
        /// </summary>
        /// <returns>Status of this node</returns>
        public NodeStatus Evaluate();
        
        /// <summary>
        /// Sets the parent of this node
        /// </summary>
        /// <param name="node">New parent of this node</param>
        public void SetParent(INode node);
        
        /// <summary>
        /// Writes the given value to the context using the given key
        /// </summary>
        /// <param name="key">Key to store the value at</param>
        /// <param name="value">Value to store</param>
        public void WriteToContext(string key, object value);
        
        /// <summary>
        /// Reads the given value from the context at the given key
        /// </summary>
        /// <param name="key">Key to read from</param>
        /// <returns>Value stored</returns>
        public object ReadFromContext(string key);
        
#if UNITY_EDITOR
        /// <summary>
        /// Current status of this node
        /// </summary>
        internal NodeStatus Status { get; }
        
        /// <summary>
        /// Name to display when showing this node
        /// </summary>
        internal string Alias { get; }
        
        /// <summary>
        /// Resets the node status before evaluation.
        /// </summary>
        internal void Reset();
#endif
    }
}