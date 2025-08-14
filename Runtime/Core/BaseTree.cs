using System.Collections.Generic;
using TreeFlow.Core.Interfaces;
using UnityEngine;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents a behaviour tree
    /// </summary>
    public abstract class BaseTree : MonoBehaviour
    {
        #region Root

        private INode root;
        
        /// <summary>
        /// Builds the behaviour tree of this tree
        /// </summary>
        /// <returns>Root node of the tree</returns>
        protected abstract INode BuildTree();
        
#if UNITY_EDITOR
        /// <summary>
        /// Gets the root of this tree
        /// </summary>
        internal INode GetRoot() => root ??= BuildTree();
#endif

        #endregion

        #region Events

        /// <summary>
        /// Creates the tree from scratch
        /// </summary>
        protected void InitializeTree() => root = BuildTree();

        /// <summary>
        /// Evaluates the tree from the root
        /// </summary>
        protected void EvaluateTree()
        {
#if UNITY_EDITOR
            var stack = new Stack<INode>();
            stack.Push(root);
            
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                current.Reset();
                
                if (current is not IParentNode parent)
                    continue;
                
                foreach (var child in parent)
                    stack.Push(child);
            }
            
#endif
            root.Evaluate();
        }

        #endregion
    }
}