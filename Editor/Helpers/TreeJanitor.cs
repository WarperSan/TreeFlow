using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Decorator;
using TreeFlow.Editor.ScriptableObjects;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that handles the organization and cleanness of a <see cref="BehaviorTreeAsset"/>
    /// </summary>
    internal static class TreeJanitor
    {
        /// <summary>
        /// Optimizes the given tree based of known rules
        /// </summary>
        public static void OptimizeTree(BehaviorTreeAsset tree)
        {
            TreeUtils.TraverseTreeFromBottom(tree, (parent, current) => {
                // Don't handle root yet
                if (parent == null)
                    return;
                
                // Don't handle leaves yet
                if (current is not IParentNode)
                    return;

                if (current is InverterNodeAsset inverter)
                {
                    var firstChild = inverter.GetChild();
                    
                    if (firstChild is InverterNodeAsset firstInverter)
                    {
                        parent.Replace(current, firstInverter.GetChild());
                        return;
                    }
                }
            });

            // Parent -> Not -> Not -> Child = Parent -> Child
            // Parent -> Selector (Empty) = Parent -> FAILURE
            // Parent -> Sequence (Empty) = Parent -> SUCCESS
            // Parent -> Selector -> FAILURE, ... = Parent -> Selector -> ...
            // Parent -> Selector -> SUCCESS / RUNNING, ... = Parent -> SUCCESS / RUNNING
            // Parent -> Sequence -> SUCCESS, ... = Parent -> Sequence -> ...
            // Parent -> Sequence -> FAILURE / RUNNING, ... = Parent -> FAILURE / RUNNING
        }
    }
}