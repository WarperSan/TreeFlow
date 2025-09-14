using System.Collections.Generic;
using NUnit.Framework;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Runtime.Nodes.Composite;
using TreeFlow.Runtime.Nodes.Leaf;

namespace TreeFlow.Tests.Editor.TreeUtils
{
    public class TraverseTreeFromTopTests
    {
        [Test]
        public void Test()
        {
            var tree = Utils.TreeUtils.CreateTree();
            var seq1 = tree.AddNode<Sequence>();
            var act1 = tree.AddNode<Action>();
            var act2 = tree.AddNode<Action>();
            var act3 = tree.AddNode<Action>();

            var root = tree.GetNode(tree.Root);

            root.Name = "ROOT";
            seq1.Name = "SEQ-1";
            act1.Name = "ACT-1";
            act2.Name = "ACT-2";
            act3.Name = "ACT-3";

            tree.AddLinks(new Dictionary<NodeAsset, ISet<NodeAsset>>
            {
                { root, new HashSet<NodeAsset> { seq1 } },
                { seq1, new HashSet<NodeAsset> { act1, act2, act3 } }
            });

            var output = "";

            TreeFlow.Editor.Helpers.TreeUtils.TraverseTreeFromTop(tree, n =>
            {
                output += n.Name + "/";
            });

            Assert.AreEqual("ROOT/SEQ-1/ACT-1/ACT-2/ACT-3/", output);
        }
    }
}