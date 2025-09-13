using NUnit.Framework;
using TreeFlow.Runtime.Core;
using TreeFlow.Runtime.Nodes.Composite;
using TreeFlow.Tests.Utils;

namespace TreeFlow.Tests.Runtime.Nodes.Composites
{
    public class SequenceNodeTests
    {
        [Test]
        public void Empty()
        {
            var sequence = new Sequence();

            Assert.AreEqual(Node.NodeStatus.SUCCESS, sequence.Process());
        }

        [Test]
        public void OneRunning()
        {
            var sequence = new Sequence();
            sequence.Attach(NodeUtils.AlwaysRunning());

            Assert.AreEqual(Node.NodeStatus.RUNNING, sequence.Process());
        }

        [Test]
        public void OneFailure()
        {
            var sequence = new Sequence();
            sequence.Attach(NodeUtils.AlwaysFailure());

            Assert.AreEqual(Node.NodeStatus.FAILURE, sequence.Process());
        }

        [Test]
        public void OneSuccess()
        {
            var sequence = new Sequence();
            sequence.Attach(NodeUtils.AlwaysSuccess());

            Assert.AreEqual(Node.NodeStatus.SUCCESS, sequence.Process());
        }

        [Test]
        public void FailureThroughSuccess()
        {
            var sequence = new Sequence();
            sequence.Attach(NodeUtils.AlwaysSuccess());
            sequence.Attach(NodeUtils.AlwaysFailure());

            Assert.AreEqual(Node.NodeStatus.FAILURE, sequence.Process());
        }

        [Test]
        public void RunningThroughSuccess()
        {
            var sequence = new Sequence();
            sequence.Attach(NodeUtils.AlwaysSuccess());
            sequence.Attach(NodeUtils.AlwaysRunning());

            Assert.AreEqual(Node.NodeStatus.RUNNING, sequence.Process());
        }

        [Test]
        public void MultipleSuccess()
        {
            var sequence = new Sequence();
            sequence.Attach(NodeUtils.AlwaysSuccess());
            sequence.Attach(NodeUtils.AlwaysSuccess());

            Assert.AreEqual(Node.NodeStatus.SUCCESS, sequence.Process());
        }
    }
}