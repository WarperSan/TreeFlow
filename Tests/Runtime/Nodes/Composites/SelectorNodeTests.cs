using NUnit.Framework;
using TreeFlow.Runtime.Core;
using TreeFlow.Runtime.Nodes.Composite;
using TreeFlow.Tests.Utils;

namespace TreeFlow.Tests.Runtime.Nodes.Composites
{
    public class SelectorNodeTests
    {
        [Test]
        public void Empty()
        {
            var selector = new Selector();

            Assert.AreEqual(Node.NodeStatus.FAILURE, selector.Process());
        }

        [Test]
        public void OneRunning()
        {
            var selector = new Selector();
            selector.Attach(NodeUtils.AlwaysRunning());

            Assert.AreEqual(Node.NodeStatus.RUNNING, selector.Process());
        }

        [Test]
        public void OneFailure()
        {
            var selector = new Selector();
            selector.Attach(NodeUtils.AlwaysFailure());

            Assert.AreEqual(Node.NodeStatus.FAILURE, selector.Process());
        }

        [Test]
        public void OneSuccess()
        {
            var selector = new Selector();
            selector.Attach(NodeUtils.AlwaysSuccess());

            Assert.AreEqual(Node.NodeStatus.SUCCESS, selector.Process());
        }

        [Test]
        public void SuccessThroughFailure()
        {
            var selector = new Selector();
            selector.Attach(NodeUtils.AlwaysFailure());
            selector.Attach(NodeUtils.AlwaysSuccess());

            Assert.AreEqual(Node.NodeStatus.SUCCESS, selector.Process());
        }

        [Test]
        public void RunningThroughFailure()
        {
            var selector = new Selector();
            selector.Attach(NodeUtils.AlwaysFailure());
            selector.Attach(NodeUtils.AlwaysRunning());

            Assert.AreEqual(Node.NodeStatus.RUNNING, selector.Process());
        }

        [Test]
        public void MultipleFailure()
        {
            var sequence = new Sequence();
            sequence.Attach(NodeUtils.AlwaysFailure());
            sequence.Attach(NodeUtils.AlwaysFailure());

            Assert.AreEqual(Node.NodeStatus.FAILURE, sequence.Process());
        }
    }
}