using System;
using NUnit.Framework;
using TreeFlow.Runtime.Core;
using TreeFlow.Runtime.Nodes.Leaf;

namespace TreeFlow.Tests.Nodes.Leaves
{
    public class ConditionNodeTests
    {
        [Test]
        public void Empty()
        {
            var condition = new Condition(null);
            
            Assert.AreEqual(Node.NodeStatus.FAILURE, condition.Process());
        }
        
        [Test]
        public void Failure()
        {
            var condition = new Condition(_ => false);
            
            Assert.AreEqual(Node.NodeStatus.FAILURE, condition.Process());
        }
        
        [Test]
        public void Success()
        {
            var condition = new Condition(_ => true);
            
            Assert.AreEqual(Node.NodeStatus.SUCCESS, condition.Process());
        }
        
        [Test]
        public void Exception()
        {
            try
            {
                var condition = new Condition(_ => throw new Exception());

                condition.Process();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
        }
    }
}