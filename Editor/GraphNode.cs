using TreeFlow.Runtime.Core;
using UnityEngine;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Data used to represent a <see cref="Node"/>
    /// </summary>
    [System.Serializable]
    public class GraphNode
    {
        /// <summary>
        /// Identifier that is unique to this instance
        /// </summary>
        public string GUID;
        
        /// <summary>
        /// 2D position of this node in the graph
        /// </summary>
        public Vector2 Position;
        
        /// <summary>
        /// Name to display for this instance
        /// </summary>
        public string Name;
    }
}