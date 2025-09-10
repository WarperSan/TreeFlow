using System;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.UIElements;
using UnityEngine;

namespace TreeFlow.Editor.ScriptableObjects
{
    /// <summary>
    /// Asset that represents a node inside <see cref="BehaviorTreeAsset"/>
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Called when <see cref="NodeView"/> needs to display this node
        /// </summary>
        public virtual void Customize(INodeView view)
        {
            view.SetDefaultTitle("Node");
        }
    }
}