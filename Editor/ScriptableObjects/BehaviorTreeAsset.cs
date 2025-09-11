using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        
        /// <inheritdoc cref="rootGUID"/>
        public string Root => rootGUID;
        
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
            
            return node;
        }

        /// <summary>
        /// Removes the given nodes from this tree
        /// </summary>
        public void RemoveNodes(ICollection<NodeAsset> nodesToRemove)
        {
            var uniqueNodesToRemove = new HashSet<NodeAsset>(nodesToRemove);
            
            for (var i = nodes.Count - 1; i >= 0; i--)
            {
                var node = nodes[i];

                if (uniqueNodesToRemove.Contains(node))
                {
                    nodes.RemoveAt(i);
                    continue;
                }
                
                if (node is not IParentNode parentNode)
                    continue;

                parentNode.Unlink(nodesToRemove);
            }
        }

        /// <summary>
        /// Moves the given nodes to the given positions
        /// </summary>
        public void MoveNodes(IDictionary<NodeAsset, Vector2> positions)
        {
            foreach (var (node, position) in positions)
                node.Position = position;
        }

        #endregion

        #region Links

        /// <summary>
        /// Adds the given links to this tree
        /// </summary>
        public void AddLinks(IDictionary<NodeAsset, ISet<NodeAsset>> newLinks)
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

        #region Editor

        [SerializeField] private Vector2 viewPosition = Vector2.zero;
        [SerializeField] private Vector2 viewScale = Vector2.one;

        /// <summary>
        /// Saves the view into this tree
        /// </summary>
        internal void SaveViewport(ITransform transform)
        {
            viewPosition = transform.position;
            viewScale = transform.scale;
        }

        /// <summary>
        /// Aligns the given view with the saved one
        /// </summary>
        internal void LoadViewport(ITransform transform)
        {
            transform.position = viewPosition;
            transform.scale = viewScale;
        }

        #endregion
        
        #region Utils
        
        /// <summary>
        /// Computes important information for later use
        /// </summary>
        internal void Compute()
        {
            foreach (var node in nodes)
                node?.Compute(this);
        }

        #endregion
    }
}