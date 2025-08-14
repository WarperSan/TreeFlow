using System.Collections;
using System.Collections.Generic;
using TreeFlow.Runtime.Core;

namespace TreeFlow.Core.Interfaces
{
    /// <summary>
    /// Defines every <see cref="BaseNode"/> that contains any children
    /// </summary>
    public interface IParentNode : IEnumerable<BaseNode>
    {
        /// <summary>
        /// Gets the children of this node
        /// </summary>
        protected IEnumerable<BaseNode> GetChildren();
        
        /// <inheritdoc/>
        IEnumerator<BaseNode> IEnumerable<BaseNode>.GetEnumerator() => GetChildren().GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }    
}