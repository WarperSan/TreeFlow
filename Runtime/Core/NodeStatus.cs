namespace TreeFlow.Runtime.Core
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
        RUNNING = 2,
        
#if UNITY_EDITOR
        /// <summary>The node has not been processed yet</summary>
        NONE = 3
#endif
    }
}