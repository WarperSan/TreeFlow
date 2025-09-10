using System;
using System.Collections.Generic;
using TreeFlow.Editor.Nodes.Core;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.ScriptableObjects
{
    /// <summary>
    /// Asset that represents a behavior tree usable by the editor
    /// </summary>
    public class BehaviorTreeAsset : ScriptableObject
    {
        /// <summary>
        /// <see cref="GUID"/> of the root node
        /// </summary>
        public string RootGUID;
        
        /// <summary>
        /// List of every node present in the tree
        /// </summary>
        [SerializeReference]
        public List<NodeAsset> Nodes = new();

        /// <summary>
        /// Adds a brand-new node with the given type to this tree 
        /// </summary>
        public T AddNode<T>() where T : NodeAsset, new()
        {
            var node = new T
            {
                GUID = GUID.Generate().ToString()
            };

            Nodes.Add(node);
            nodeByGUID.TryAdd(node.GUID, node);
            
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
                nodeByGUID.Remove(node.GUID);
            }
        }

        /// <summary>
        /// Sets the position of the given nodes to the given position
        /// </summary>
        public void SetPositions(Dictionary<string, Vector2> positions)
        {
            foreach (var (guid, position) in positions)
            {
                if (!nodeByGUID.TryGetValue(guid, out var node))
                    continue;
                
                node.Position = position;
            }
        }

        #region Utils
        
        [NonSerialized] private readonly Dictionary<string, NodeAsset> nodeByGUID = new();
        [NonSerialized] private readonly Dictionary<string, HashSet<string>> linksByGUID = new();

        /// <summary>
        /// Computes important information for later use
        /// </summary>
        public void Compute()
        {
            nodeByGUID.Clear();

            foreach (var node in Nodes)
            {
                if (node is null)
                    continue;

                nodeByGUID.TryAdd(node.GUID, node);
                node.IsRoot = node.GUID == RootGUID;
            }
        }

        #endregion
    }
}