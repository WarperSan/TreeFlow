using System;
using NUnit.Framework;
using TreeFlow.Runtime.Core;
using Action = TreeFlow.Runtime.Nodes.Leaf.Action;

namespace TreeFlow.Tests.Runtime.Nodes.Leaves
{
    public class ActionNodeTests
    {
        [Test]
        public void Empty()
        {
            var action = new Action(null);
            
            Assert.AreEqual(Node.NodeStatus.FAILURE, action.Process());
        }
        
        [Test]
        public void Failure()
        {
            var action = new Action(_ => Node.NodeStatus.FAILURE);
            
            Assert.AreEqual(Node.NodeStatus.FAILURE, action.Process());
        }
        
        [Test]
        public void Success()
        {
            var action = new Action(_ => Node.NodeStatus.SUCCESS);
            
            Assert.AreEqual(Node.NodeStatus.SUCCESS, action.Process());
        }
        
        [Test]
        public void Running()
        {
            var action = new Action(_ => Node.NodeStatus.RUNNING);
            
            Assert.AreEqual(Node.NodeStatus.RUNNING, action.Process());
        }
        
        [Test]
        public void Exception()
        {
            try
            {
                var action = new Action(_ => throw new Exception());

                action.Process();
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Pass();
            }
        }
    }
}