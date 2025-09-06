using System.Collections;
using System.Collections.Generic;

namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing children
    /// </summary>
    public abstract class Composite : Node, IEnumerable<Node>
    {
        protected Composite(params Node[] nodes) => Attach(nodes);

        #region Children

        private readonly List<Node> children = new();

        /// <summary>
        /// Attaches the children to this node
        /// </summary>
        /// <param name="nodes">Nodes to attach to this node</param>
        public void Attach(params Node[] nodes)
        {
            foreach (var child in nodes)
                children.Add(child);
        }

        #endregion

        #region IEnumerable

        /// <inheritdoc/>
        public IEnumerator<Node> GetEnumerator() => children.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}