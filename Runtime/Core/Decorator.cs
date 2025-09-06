namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node containing only one child
    /// </summary>
    public abstract class Decorator : Node
    {
        protected Decorator(Node child)
        {
            ReplaceChild(child);
        }

        /// <summary>
        /// Current child of this node
        /// </summary>
        protected Node Child { get; private set; }

        /// <summary>
        /// Replaces the current child of this node with the given child
        /// </summary>
        public void ReplaceChild(Node newChild) => Child = newChild;
    }
}