using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Composite;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.Nodes.Decorator;
using TreeFlow.Editor.Nodes.Leaf;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that allows to customize <see cref="INodeView"/>
    /// </summary>
    internal static class NodeCustomizer
    {
        /// <summary>
        /// Customizes the given <see cref="INodeView"/>
        /// </summary>
        public static void Customize(INodeView view, NodeAsset node)
        {
            // Root Style
            if (node.IsRoot)
            {
                view.SetDefaultTitle("Root");
                view.SetColor(50, 50, 50);
                view.AddOutputPort(true);
                return;
            }

            // General Styles
            switch (node)
            {
                case CompositeNodeAsset:
                    view.AddInputPort();
                    view.AddOutputPort(true);
                    break;
                case DecoratorNodeAsset:
                    view.AddInputPort();
                    view.AddOutputPort();
                    break;
                case LeafNodeAsset:
                    view.AddInputPort();
                    break;
            }

            // Specific styles
            switch (node)
            {
                case SelectorNodeAsset:
                    view.SetDefaultTitle("Selector");
                    view.SetColor(17, 93, 168);
                    break;
                case SequenceNodeAsset:
                    view.SetDefaultTitle("Sequence");
                    view.SetColor(18, 152, 87);
                    break;
                case ParallelNodeAsset:
                    view.SetDefaultTitle("Parallel");
                    view.SetColor(156, 162, 71);
                    break;
                case InverterNodeAsset:
                    view.SetDefaultTitle("Inverter");
                    view.SetColor(155, 89, 182);
                    break;
                case RepeatNodeAsset:
                    view.SetDefaultTitle("Repeat");
                    view.SetColor(22, 160, 133);
                    break;
                case ActionNodeAsset:
                    view.SetDefaultTitle("Action");
                    view.SetColor(182, 59, 47);
                    break;
                case ConditionNodeAsset:
                    view.SetDefaultTitle("Condition");
                    view.SetColor(44, 62, 80);
                    break;
            }
        }
    }
}