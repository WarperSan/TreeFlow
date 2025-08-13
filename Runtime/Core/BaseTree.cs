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