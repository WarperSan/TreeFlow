using System;
using System.Collections.Generic;
using System.IO;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Composite;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.ScriptableObjects;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that holds useful tools to query <see cref="BehaviorTreeAsset"/>
    /// </summary>
    internal static class TreeUtils
    {
        /// <summary>
        /// Creates a new <see cref="BehaviorTreeAsset"/> to the given file
        /// </summary>
        /// <remarks>
        /// If <see cref="path"/> is omitted, the asset will be created in-memory only
        /// </remarks>
        internal static BehaviorTreeAsset CreateTree(string path = null)
        {
            var asset = ScriptableObject.CreateInstance<BehaviorTreeAsset>();

            var root = asset.AddNode<SelectorNodeAsset>();
            asset.PromotesToRoot(root);

            // ReSharper disable once InvertIf
            if (path != null)
            {
                path = Path.GetRelativePath(Path.GetDirectoryName(Application.dataPath), path);
                Resources.SaveChanges(asset);
                Resources.Save(asset, path);
            }
            
            return asset;
        }
        
        /// <summary>
        /// Traverses the given tree from the top to the bottom
        /// </summary>
        /// <remarks>
        /// The parent calls the callback, then the children.
        /// </remarks>
        internal static void TraverseTreeFromTop(BehaviorTreeAsset tree, Action<NodeAsset> callback)
        {
            if (callback == null)
                return;
            
            var queue = new Queue<NodeAsset>();
            queue.Enqueue(tree.GetNode(tree.Root));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                
                if (current == null)
                    continue;
                
                callback.Invoke(current);

                if (current is not IParentNode parentNode)
                    continue;

                foreach (var child in parentNode.GetChildren(tree))
                    queue.Enqueue(child);
            }
        }

        /// <summary>
        /// Traverses the given tree from the bottom to the top
        /// </summary>
        /// <remarks>
        /// The children call the callback, then the parent.
        /// </remarks>
        internal static void TraverseTreeFromBottom(BehaviorTreeAsset tree, Action<IParentNode, NodeAsset> callback)
        {
            if (callback == null)
                return;

            var root = tree.GetNode(tree.Root);
            
            if (root == null)
                return;
            
            var stack = new Stack<NodeAsset>();
            var visited = new HashSet<NodeAsset>();
            var parentByNode = new Dictionary<NodeAsset, IParentNode>();

            stack.Push(root);
            parentByNode.TryAdd(root, null);

            while (stack.Count > 0)
            {
                var current = stack.Peek();

                if (!visited.Add(current) || current is not IParentNode parentNode)
                {
                    stack.Pop();
                    callback.Invoke(parentByNode.GetValueOrDefault(current), current);
                    continue;
                }

                var cachedChildren = new Stack<NodeAsset>();
                
                foreach (var child in parentNode.GetChildren(tree))
                {
                    if (child == null)
                        continue;

                    cachedChildren.Push(child);
                    parentByNode.TryAdd(child, parentNode);
                }

                while (cachedChildren.Count > 0)
                    stack.Push(cachedChildren.Pop());
            }
        }
    }
}