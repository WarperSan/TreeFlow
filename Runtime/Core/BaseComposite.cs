using System.Collections.Generic;
using TreeFlow.Runtime.Core.Interfaces;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing children
    /// </summary>
    public abstract class BaseComposite : BaseNode, IParentNode
    {
        protected BaseComposite(params INode[] nodes) => Attach(nodes);

        #region Hierarchy

        private readonly List<INode> children = new();

        /// <summary>
        /// Attaches the children to this node
        /// </summary>
        /// <param name="nodes">Nodes to attach to this node</param>
        public void Attach(params INode[] nodes)
        {
            foreach (var child in nodes)
            {
                child.SetParent(this);
                children.Add(child);
            }
        }

        #endregion

        #region IParentNode

        /// <inheritdoc/>
        IEnumerable<INode> IParentNode.GetChildren() => children;

        #endregion
    }
}