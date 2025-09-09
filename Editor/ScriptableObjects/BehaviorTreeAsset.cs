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
        [SerializeField] public List<NodeAsset> Nodes = new();

        /// <summary>
        /// Adds a brand-new node with the given type to this tree 
        /// </summary>
        public T AddNode<T>() where T : NodeAsset, new()
        {
            var node = CreateInstance<T>();
            node.GUID = GUID.Generate().ToString();
            Nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            return node;
        }

        /// <summary>
        /// Removes the given nodes from this tree
        /// </summary>
        public void RemoveNodes(IEnumerable<string> guids)
        {
            var uniqueGUIDs = new HashSet<string>(guids);

            for (var i = Nodes.Count - 1; i >= 0; i--)
            {
                var node = Nodes[i];

                if (!uniqueGUIDs.Contains(node.GUID))
                    continue;

                Nodes.RemoveAt(i);
                AssetDatabase.RemoveObjectFromAsset(node);
            }
        }

        /// <summary>
        /// Sets the position of the given nodes to the given position
        /// </summary>
        public void SetPositions(Dictionary<string, Vector2> positions)
        {
            foreach (var node in Nodes)
            {
                if (!positions.TryGetValue(node.GUID, out var position))
                    continue;
                
                node.Position = position;
            }
        }
    }
}