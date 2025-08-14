using System.Collections.Generic;
using TreeFlow.Core.Interfaces;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node in a tree
    /// </summary>
    public abstract class BaseNode : INode
    {
        #region Evaluation

        /// <summary>
        /// Evaluates this node
        /// </summary>
        /// <returns>Status of this node</returns>
        public abstract NodeStatus Evaluate();

        #endregion

        #region Hierarchy
        
        private INode parent;

        /// <inheritdoc/>
        public void SetParent(INode node) => parent = node;

        #endregion

        #region Data Context

        private readonly Dictionary<string, object> dataContext = new();

        /// <inheritdoc/>
        public void WriteToContext(string key, object value)
        {
            dataContext[key] = value;
        }

        /// <inheritdoc/>
        public object ReadFromContext(string key)
        {
            if (dataContext.TryGetValue(key, out var value))
                return value;
            
            return parent?.ReadFromContext(key);
        }

        #endregion
    }
}