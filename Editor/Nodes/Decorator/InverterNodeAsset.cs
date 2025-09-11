using System;
using TreeFlow.Editor.Nodes.Core;

namespace TreeFlow.Editor.Nodes.Decorator
{
    [Serializable]
    public class InverterNodeAsset : DecoratorNodeAsset
    {
        public NodeAsset GetChild()
        {
            NodeAsset child = null;

            var enumerator = Children.GetEnumerator();
            
            if (enumerator.MoveNext())
                child = Tree?.GetNode(enumerator.Current);
                    
            enumerator.Dispose();
            
            return child;
        }
    }
}