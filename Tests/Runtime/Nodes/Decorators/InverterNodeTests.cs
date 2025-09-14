using NUnit.Framework;
using TreeFlow.Runtime.Core;
using TreeFlow.Runtime.Nodes.Decorator;
using TreeFlow.Tests.Utils;

namespace TreeFlow.Tests.Runtime.Nodes.Decorators
{
    public class InverterNodeTests
    {
        [Test]
        public void SuccessToFailure()
        {
            var inverter = new Inverter(NodeUtils.AlwaysSuccess());

            Assert.AreEqual(Node.NodeStatus.FAILURE, inverter.Process());
        }

        [Test]
        public void FailureToSuccess()
        {
            var inverter = new Inverter(NodeUtils.AlwaysFailure());

            Assert.AreEqual(Node.NodeStatus.SUCCESS, inverter.Process());
        }

        [Test]
        public void RunningToRunning()
        {
            var inverter = new Inverter(NodeUtils.AlwaysRunning());

            Assert.AreEqual(Node.NodeStatus.RUNNING, inverter.Process());
        }
    }
}