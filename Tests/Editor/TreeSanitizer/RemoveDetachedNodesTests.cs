using NUnit.Framework;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Composite;

namespace TreeFlow.Tests.Editor.TreeSanitizer
{
    public class RemoveDetachedNodesTests
    {
        [Test]
        public void RootAndNode()
        {
            var tree = Utils.TreeUtils.CreateTree();
            var node = tree.AddNode<SelectorNodeAsset>().GUID;
            
            tree.Compute();
            TreeFlow.Editor.Helpers.TreeSanitizer.RemoveDetachedNodes(tree);
            
            Assert.AreEqual(null, tree.GetNode(node));
        }
        
        [Test]
        public void RootWithChild()
        {
            var tree = Utils.TreeUtils.CreateTree();
            var node1 = tree.AddNode<SelectorNodeAsset>();
            var node2 = tree.AddNode<SelectorNodeAsset>();

            var root = tree.GetNode(tree.Root);
            
            if (root is IParentNode parent)
                parent.Link(node2);
            
            tree.Compute();
            TreeFlow.Editor.Helpers.TreeSanitizer.RemoveDetachedNodes(tree);
            
            Assert.AreEqual(null, tree.GetNode(node1.GUID));
            Assert.AreEqual(node2, tree.GetNode(node2.GUID));
        }
    }
}