using UnityEngine;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents a behaviour tree
    /// </summary>
    public abstract class BaseTree : MonoBehaviour
    {
        #region Root

        private BaseNode root;
        
        /// <summary>
        /// Builds the behaviour tree of this tree
        /// </summary>
        /// <returns>Root node of the tree</returns>
        protected abstract BaseNode BuildTree();
        
#if UNITY_EDITOR
        /// <summary>
        /// Gets the root of this tree
        /// </summary>
        internal BaseNode GetRoot() => root ??= BuildTree();
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