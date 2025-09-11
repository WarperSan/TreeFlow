using NUnit.Framework;
using TreeFlow.Runtime.Core;
using TreeFlow.Runtime.Nodes.Composite;
using TreeFlow.Tests.Tests.Utils;

namespace TreeFlow.Tests.Runtime.Nodes.Composites
{
    public class ParallelNodeTests
    {
        [Test]
        public void Empty()
        {
            var parallel = new Parallel(0);
            
            Assert.AreEqual(Node.NodeStatus.SUCCESS, parallel.Process());
        }

        [Test]
        public void EmptyWithThreshold()
        {
            var parallel = new Parallel(1);
            
            Assert.AreEqual(Node.NodeStatus.FAILURE, parallel.Process());
        }

        [Test]
        public void MostlySuccess()
        {
            var parallel = new Parallel(2);
            parallel.Attach(NodeUtils.AlwaysSuccess());
            parallel.Attach(NodeUtils.AlwaysSuccess());
            parallel.Attach(NodeUtils.AlwaysFailure());
            
            Assert.AreEqual(Node.NodeStatus.SUCCESS, parallel.Process());
        }
        
        [Test]
        public void MostlyFailure()
        {
            var parallel = new Parallel(2);
            parallel.Attach(NodeUtils.AlwaysSuccess());
            parallel.Attach(NodeUtils.AlwaysFailure());
            parallel.Attach(NodeUtils.AlwaysFailure());
            
            Assert.AreEqual(Node.NodeStatus.FAILURE, parallel.Process());
        }

        [Test]
        public void ThresholdNotReached()
        {
            var parallel = new Parallel(2);
            parallel.Attach(NodeUtils.AlwaysSuccess());
            parallel.Attach(NodeUtils.AlwaysRunning());
            parallel.Attach(NodeUtils.AlwaysRunning());
            parallel.Attach(NodeUtils.AlwaysFailure());
            
            Assert.AreEqual(Node.NodeStatus.RUNNING, parallel.Process());
        }
    }
}