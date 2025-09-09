using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.ScriptableObjects
{
    /// <summary>
    /// Asset that represents a behavior tree usable by the editor
    /// </summary>
    [CreateAssetMenu(menuName = "TreeFlow/Behavior Tree")]
    public class BehaviorTreeAsset : ScriptableObject
    {
        /// <summary>
        /// <see cref="GraphNode.GUID"/> of the root node
        /// </summary>
        public string RootGUID;
        
        /// <summary>
        /// List of every node present in the tree
        /// </summary>
        private readonly List<NodeAsset> nodes = new();

        /// <inheritdoc cref="BehaviorTreeAsset.nodes"/>
        public IReadOnlyList<NodeAsset> Nodes => nodes;

        /// <summary>
        /// Adds a brand-new node with the given type to this tree 
        /// </summary>
        public T AddNode<T>() where T : NodeAsset, new()
        {
            var node = CreateInstance<T>();
            node.GUID = GUID.Generate().ToString();
            nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Removes the given nodes from this tree
        /// </summary>
        public void RemoveNodes(IEnumerable<string> guids)
        {
            var uniqueGUIDs = new HashSet<string>(guids);

            for (var i = nodes.Count - 1; i >= 0; i--)
            {
                if (!uniqueGUIDs.Contains(nodes[i].GUID))
                    continue;

                nodes.RemoveAt(i);
            }
        }

        /// <summary>
        /// Sets the position of the given nodes to the given position
        /// </summary>
        public void SetPositions(Dictionary<string, Vector2> positions)
        {
            foreach (var node in nodes)
            {
                if (!positions.TryGetValue(node.GUID, out var position))
                    continue;
                
                node.Position = position;
            }
        }
    }
}