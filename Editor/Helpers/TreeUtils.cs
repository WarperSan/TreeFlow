using System;
using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.ScriptableObjects;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that holds useful tools to query <see cref="BehaviorTreeAsset"/>
    /// </summary>
    internal static class TreeUtils
    {
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

            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Peek();

                if (!visited.Add(current) || current is not IParentNode parentNode)
                {
                    stack.Pop();
                    stack.TryPeek(out var parent);
                    callback.Invoke(parent as IParentNode, current);
                    continue;
                }

                foreach (var child in parentNode.Children)
                {
                    var childNode = tree.GetNode(child);
                    
                    if (childNode == null)
                        continue;
                    
                    stack.Push(childNode);
                }
            }
        }
    }
}