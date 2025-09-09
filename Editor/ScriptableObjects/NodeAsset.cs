using UnityEngine;

namespace TreeFlow.Editor.ScriptableObjects
{
    /// <summary>
    /// Asset that represents a node inside <see cref="BehaviorTreeAsset"/>
    /// </summary>
    public abstract class NodeAsset : ScriptableObject
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

    public abstract class CompositeNodeAsset : NodeAsset
    {
    }
    
    public sealed class SequenceNodeAsset : CompositeNodeAsset { }
}