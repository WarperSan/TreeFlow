using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that handles the sanitization of <see cref="BehaviorTreeAsset"/>
    /// </summary>
    internal static class TreeSanitizer
    {
        /// <summary>
        /// Removes every node that is not attached to <see cref="BehaviorTreeAsset.Root"/>
        /// </summary>
        public static void RemoveDetachedNodes(BehaviorTreeAsset tree)
        {
            var indexedNodes = new Dictionary<string, NodeAsset>();

            foreach (var node in tree.Nodes)
                indexedNodes.TryAdd(node.GUID, node);
            
            TreeUtils.TraverseTreeFromTop(tree, n => indexedNodes.Remove(n.GUID));
            
            tree.RemoveNodes(indexedNodes.Values);

            EditorUtility.SetDirty(tree);
        }
        
        /// <summary>
        /// Fixes every node reference in the graph
        /// </summary>
        public static void FixChildNodes(BehaviorTreeAsset tree)
        {
            var root = tree.GetNode(tree.Root);

            if (root == null)
            {
                Debug.LogWarning("No root was found in the tree.");
                return;
            }
            
            var navigationStack = new Stack<NodeAsset>();
            navigationStack.Push(root);

            while (navigationStack.Count > 0)
            {
                var current = navigationStack.Pop();

                if (current is not IParentNode parent)
                    continue;
                
                if (parent.Count < 1)
                    continue;

                var children = new List<NodeAsset>();

                foreach (var child in parent.GetChildren(tree))
                {
                    if (child == null)
                        continue;
                    
                    children.Add(child);
                    navigationStack.Push(child);
                }
                
                children.Sort((a, b) => a.Position.x.CompareTo(b.Position.x));

                var uniqueChildren = new HashSet<string>();
                
                foreach (var child in children)
                {
                    parent.Unlink(child);
                 
                    if (!uniqueChildren.Add(child.GUID))
                        continue;
                    
                    parent.Link(child);
                }
            }

            EditorUtility.SetDirty(tree);
        }
    }
}