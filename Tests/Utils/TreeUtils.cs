using TreeFlow.Editor.Nodes.Composite;
using TreeFlow.Editor.ScriptableObjects;
using UnityEngine;

namespace TreeFlow.Tests.Utils
{
    public static class TreeUtils
    {
        public static BehaviorTreeAsset Tree()
        {
            var tree = ScriptableObject.CreateInstance<BehaviorTreeAsset>();

            var root = tree.AddNode<SelectorNodeAsset>();
            tree.PromotesToRoot(root);
            
            return tree;
        }
    }
}