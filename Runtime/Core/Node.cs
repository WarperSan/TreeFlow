namespace TreeFlow.Runtime.Core
{
    /// <summary>
    /// Class that represents any node in a tree
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Statuses of the node
        /// </summary>
        public enum NodeStatus
        {
            /// <summary>The node has failed to achieve its goal</summary>
            FAILURE = 0,

            /// <summary>The node has achieved its goal</summary>
            SUCCESS = 1,

            /// <summary>The node is still processing, and the outcome is undetermined</summary>
            RUNNING = 2
        }

        #region Processing

        /// <summary>
        /// Processes the current tick of this node
        /// </summary>
        /// <returns>Current status of this node</returns>
        public abstract NodeStatus Process();

        #endregion
    }
}