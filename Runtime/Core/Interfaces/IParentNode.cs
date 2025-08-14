using System.Collections;
using System.Collections.Generic;

namespace TreeFlow.Runtime.Core.Interfaces
{
    /// <summary>
    /// Defines every <see cref="INode"/> that contains any children
    /// </summary>
    public interface IParentNode : IEnumerable<INode>
    {
        /// <summary>
        /// Gets the children of this node
        /// </summary>
        protected IEnumerable<INode> GetChildren();
        
        /// <inheritdoc/>
        IEnumerator<INode> IEnumerable<INode>.GetEnumerator() => GetChildren().GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }    
}