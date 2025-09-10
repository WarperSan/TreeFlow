using System;
using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
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
        #region Root

        /// <summary>
        /// <see cref="GUID"/> of the root node
        /// </summary>
        [SerializeField] private string rootGUID;
        
        /// <summary>
        /// Checks if the given node is the root of the tree
        /// </summary>
        public bool IsRoot(NodeAsset node) => node?.GUID == rootGUID;

        /// <summary>
        /// Promotes the given node to the tree's root
        /// </summary>
        internal void PromotesToRoot(NodeAsset node)
        {
            rootGUID = node.GUID;
            node.Compute(this);
        }

        #endregion
        
        #region Nodes

        /// <summary>
        /// List of every node present in the tree
        /// </summary>
        [SerializeReference] private List<NodeAsset> nodes = new();
        
        /// <inheritdoc cref="nodes"/>
        public IReadOnlyList<NodeAsset> Nodes => nodes;

        /// <summary>
        /// Adds a brand-new node with the given type to this tree 
        /// </summary>
        public T AddNode<T>() where T : NodeAsset, new()
        {
            var node = new T
            {
                GUID = GUID.Generate().ToString()
            };

            nodes.Add(node);
            nodeByGUID.TryAdd(node.GUID, node);
            
            return node;
        }

        /// <summary>
        /// Removes the given nodes from this tree
        /// </summary>
        public void RemoveNodes(IEnumerable<NodeAsset> nodesToRemove)
        {
            var uniqueGUIDs = new HashSet<string>();
            
            foreach (var node in nodesToRemove)
                uniqueGUIDs.Add(node.GUID);

            for (var i = nodes.Count - 1; i >= 0; i--)
            {
                var node = nodes[i];

                if (!uniqueGUIDs.Contains(node.GUID))
                    continue;

                nodes.RemoveAt(i);
                nodeByGUID.Remove(node.GUID);
            }
        }

        /// <summary>
        /// Moves the given nodes to the given positions
        /// </summary>
        public void MoveNodes(Dictionary<NodeAsset, Vector2> positions)
        {
            foreach (var (node, position) in positions)
                node.Position = position;
        }

        #endregion

        #region Links

        /// <summary>
        /// Adds the given links to this tree
        /// </summary>
        public void AddLinks(Dictionary<NodeAsset, HashSet<NodeAsset>> newLinks)
        {
            foreach (var (startNode, endNodes) in newLinks)
            {
                if (startNode is not IParentNode parentNode)
                    continue;

                foreach (var endNode in endNodes)
                    parentNode.Link(endNode);
            }
        }

        #endregion
        
        #region Utils
        
        [NonSerialized] private readonly Dictionary<string, NodeAsset> nodeByGUID = new();

        /// <summary>
        /// Computes important information for later use
        /// </summary>
        internal void Compute()
        {
            nodeByGUID.Clear();

            foreach (var node in nodes)
            {
                if (node is null)
                    continue;

                nodeByGUID.TryAdd(node.GUID, node);
                node.Compute(this);
            }
        }

        #endregion
    }
}