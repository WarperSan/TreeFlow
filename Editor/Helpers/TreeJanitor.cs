using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.ScriptableObjects;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that handles the organization and cleanness of a <see cref="BehaviorTreeAsset"/>
    /// </summary>
    internal static class TreeJanitor
    {
        /// <summary>
        /// Removes every node that is not connected to the root
        /// </summary>
        public static void RemoveUnusedNodes(BehaviorTreeAsset tree)
        {
            var root = tree.GetNode(tree.Root);
            var nodes = new List<NodeAsset>(tree.Nodes);

            if (root == null)
            {
                Debug.LogWarning("No root was found in the tree.");
                tree.RemoveNodes(nodes);
                return;
            }
            
            var navigationStack = new Stack<NodeAsset>();
            navigationStack.Push(root);

            while (navigationStack.Count > 0)
            {
                var current = navigationStack.Pop();

                nodes.Remove(current);
            
                if (current is not IParentNode parent)
                    continue;

                foreach (var child in parent.Children)
                {
                    var childNode = tree.GetNode(child);
                    
                    if (childNode == null)
                        continue;
                    
                    if (!nodes.Contains(childNode))
                        continue;

                    nodes.Remove(childNode);
                    navigationStack.Push(childNode);
                }
            }

            tree.RemoveNodes(nodes);
        }

        /// <summary>
        /// Orders every node reference based on their position in the graph
        /// </summary>
        public static void OrderChildNodes(BehaviorTreeAsset tree)
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

                foreach (var childGUID in parent.Children)
                {
                    var child = tree.GetNode(childGUID);
                    
                    if (child == null)
                        continue;
                    
                    children.Add(child);
                    navigationStack.Push(child);
                }
                
                children.Sort((a, b) => a.Position.x.CompareTo(b.Position.x));

                foreach (var child in children)
                {
                    parent.Unlink(child);
                    parent.Link(child);
                }
            }
        }
    }
}