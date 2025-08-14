using System.Collections.Generic;
using TreeFlow.Runtime.Core.Interfaces;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node in a tree
    /// </summary>
    public abstract partial class BaseNode : INode
    {
        #region Evaluation

        /// <summary>
        /// Evaluates this node
        /// </summary>
        /// <returns>Status of this node</returns>
        public NodeStatus Evaluate()
        {
            var status = OnEvaluate();
            OnAfterEvaluate(status);
            return status;

        }
        
        /// <summary>
        /// Called when this node gets evaluated
        /// </summary>
        /// <returns>Status of this node</returns>
        protected abstract NodeStatus OnEvaluate();
        
        partial void OnAfterEvaluate(NodeStatus status);

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

    /// <summary>
    /// Editor-only logic for BaseNode.
    /// </summary>
    public abstract partial class BaseNode
    {
#if UNITY_EDITOR
        private NodeStatus _status;
        private string _alias;

        /// <inheritdoc/>
        NodeStatus INode.Status => _status;
        
        /// <inheritdoc/>
        void INode.Reset() => _status = NodeStatus.NONE;

        /// <summary>
        /// Updates the node status after evaluation.
        /// </summary>
        partial void OnAfterEvaluate(NodeStatus status) => _status = status;
#endif

#if UNITY_EDITOR
        /// <inheritdoc/>
        string INode.Alias => _alias ?? Alias;

        /// <inheritdoc cref="INode.Alias"/>
        protected virtual string Alias => GetType().Name;
#endif

        /// <summary>
        /// Sets the alias of this node
        /// </summary>
        public INode SetAlias(string alias)
        {
#if UNITY_EDITOR
            _alias = alias;
#endif
            return this;
        }
    }
}