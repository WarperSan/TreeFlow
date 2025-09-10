using System.Collections.Generic;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.ScriptableObjects;

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
            var nodesByGuid = new Dictionary<string, NodeAsset>();

            foreach (var node in tree.Nodes)
                nodesByGuid.TryAdd(node.GUID, node);

            if (nodesByGuid.ContainsKey(tree.Root))
            {
                var navigationStack = new Stack<NodeAsset>();
                navigationStack.Push(nodesByGuid[tree.Root]);

                while (navigationStack.Count > 0)
                {
                    var current = navigationStack.Pop();

                    nodesByGuid.Remove(current.GUID);
                
                    if (current is not IParentNode parent)
                        continue;

                    foreach (var child in parent.Children)
                    {
                        if (!nodesByGuid.Remove(child, out var nodeAsset))
                            continue;

                        navigationStack.Push(nodeAsset);
                    }
                }
            }

            tree.RemoveNodes(nodesByGuid.Values);
        }
    }
}