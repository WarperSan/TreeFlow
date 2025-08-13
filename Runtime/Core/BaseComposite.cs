using System.Collections;
using System.Collections.Generic;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing children
    /// </summary>
    public abstract class BaseComposite : BaseNode, IEnumerable<BaseNode>
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

        #endregion

        #region IEnumerable

        /// <inheritdoc/>
        public IEnumerator<BaseNode> GetEnumerator() => children.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}