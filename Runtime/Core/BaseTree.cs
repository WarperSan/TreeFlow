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
        
        #region MonoBehaviour

        protected void Awake()
        {
            root = BuildTree();
        }

        protected void Update()
        {
            root.Evaluate();
        }
        
        #endregion
    }
}