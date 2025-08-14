using System.Collections.Generic;
using TreeFlow.Core.Interfaces;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing children
    /// </summary>
    public abstract class BaseComposite : BaseNode, IParentNode
    {
        protected BaseComposite(params BaseNode[] nodes) => Attach(nodes);

        #region Hierarchy

        private readonly List<BaseNode> children = new();

        /// <summary>
        /// Attaches the children to this node
        /// </summary>
        /// <param name="nodes">Nodes to attach to this node</param>
        public void Attach(params BaseNode[] nodes)
        {
            foreach (var child in nodes)
            {
                child.SetParent(this);
                children.Add(child);
            }
        }
        
#if UNITY_EDITOR
        /// <inheritdoc/>
        internal override void Reset()
        {
            base.Reset();
            
            foreach (var child in children)
                child.Reset();
        }
#endif
        #endregion

        #region IParentNode

        /// <inheritdoc/>
        IEnumerable<BaseNode> IParentNode.GetChildren() => children;

        #endregion
    }
}