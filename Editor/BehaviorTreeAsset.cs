using System.Collections.Generic;
using UnityEngine;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Asset that represents a behavior tree usable by the editor
    /// </summary>
    [CreateAssetMenu(menuName = "TreeFlow/Behavior Tree")]
    public class BehaviorTreeAsset : ScriptableObject
    {
        /// <summary>
        /// List of every node present in the tree
        /// </summary>
        public List<GraphNode> Nodes;
    }
}