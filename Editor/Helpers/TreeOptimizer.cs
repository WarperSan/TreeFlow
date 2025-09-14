using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TreeFlow.Editor.Interfaces;
using TreeFlow.Editor.Nodes.Composite;
using TreeFlow.Editor.Nodes.Core;
using TreeFlow.Editor.Nodes.Decorator;
using TreeFlow.Editor.ScriptableObjects;
using UnityEditor;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    ///     Class that handles the optimization of a <see cref="BehaviorTreeAsset" />
    /// </summary>
    internal static class TreeOptimizer
    {
        /// <summary>
        ///     Optimizes the given tree based of known rules
        /// </summary>
        [SuppressMessage("ReSharper", "InvertIf")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        [SuppressMessage("ReSharper", "RedundantJumpStatement")]
        public static void Optimize(BehaviorTreeAsset tree)
        {
            TreeUtils.TraverseTreeFromBottom(tree, (parent, current) =>
            {
                // Don't optimize root
                if (parent == null || current.IsRoot)
                    return;

                if (current is InverterNodeAsset inverter)
                {
                    if (DoubleInverter(tree, parent, inverter))
                        return;

                    if (SimplifyInverter(tree, parent, inverter))
                        return;
                }

                if (current is SelectorNodeAsset selector)
                {
                    if (EmptySelector(tree, parent, selector))
                        return;

                    SkipSelectorFailures(tree, parent, selector);

                    if (SimplifySelector(tree, parent, selector))
                        return;

                    if (ForceSelectorStatus(tree, parent, selector))
                        return;
                }

                if (current is SequenceNodeAsset sequence)
                {
                    if (EmptySequence(tree, parent, sequence))
                        return;

                    SkipSequenceSuccesses(tree, parent, sequence);

                    if (SimplifySequence(tree, parent, sequence))
                        return;

                    if (ForceSequenceStatus(tree, parent, sequence))
                        return;
                }
            });

            var tempNodes = new List<NodeAsset>();

            TreeUtils.TraverseTreeFromTop(tree, current =>
            {
                if (current is AlwaysFailureNodeAsset or AlwaysRunningNodeAsset or AlwaysSuccessNodeAsset)
                    tempNodes.Add(current);
            });

            tree.RemoveNodes(tempNodes);

            EditorUtility.SetDirty(tree);
        }

        #region Definitions

        private static void ReplaceSelector(BehaviorTreeAsset tree, IParentNode parent, SelectorNodeAsset selector)
        {
            var replacement = tree.AddNode<AlwaysFailureNodeAsset>();
            parent.Replace(selector, replacement);
            tree.RemoveNodes(new[] { selector });
        }

        private static void ReplaceSequence(BehaviorTreeAsset tree, IParentNode parent, SequenceNodeAsset sequence)
        {
            var replacement = tree.AddNode<AlwaysSuccessNodeAsset>();
            parent.Replace(sequence, replacement);
            tree.RemoveNodes(new[] { sequence });
        }

        #endregion

        #region Rules

        // Parent -> Not -> Not -> Child = Parent -> Child
        private static bool DoubleInverter(BehaviorTreeAsset tree, IParentNode parent, InverterNodeAsset inverter)
        {
            var firstChild = inverter.GetChild(tree);

            if (firstChild is not InverterNodeAsset childInverter)
                return false;

            parent.Replace(inverter, childInverter.GetChild(tree));
            tree.RemoveNodes(new[] { inverter, childInverter });
            return true;
        }

        // Parent -> Not -> FAILURE = Parent -> SUCCESS
        // Parent -> Not -> SUCCESS = Parent -> FAILURE
        // Parent -> Not -> RUNNING = Parent -> RUNNING
        private static bool SimplifyInverter(BehaviorTreeAsset tree, IParentNode parent, InverterNodeAsset inverter)
        {
            var child = inverter.GetChild(tree);

            var replacement = child switch
            {
                AlwaysSuccessNodeAsset => tree.AddNode<AlwaysFailureNodeAsset>(),
                AlwaysFailureNodeAsset => tree.AddNode<AlwaysSuccessNodeAsset>(),
                AlwaysRunningNodeAsset => child,
                _ => null
            };

            if (replacement == null)
                return false;

            parent.Replace(inverter, replacement);
            tree.RemoveNodes(new[] { inverter });
            return true;
        }

        // Parent -> Selector (Empty) = Parent -> FAILURE
        private static bool EmptySelector(BehaviorTreeAsset tree, IParentNode parent, SelectorNodeAsset selector)
        {
            if (selector.Count > 0)
                return false;

            ReplaceSelector(tree, parent, selector);
            return true;
        }

        // Parent -> Selector -> FAILURE, ... = Parent -> Selector -> ...
        private static void SkipSelectorFailures(BehaviorTreeAsset tree, IParentNode parent, SelectorNodeAsset selector)
        {
            var childrenToRemove = new List<NodeAsset>();

            foreach (var child in (selector as IParentNode).GetChildren(tree))
            {
                if (child is not AlwaysFailureNodeAsset)
                    continue;

                childrenToRemove.Add(child);
                selector.Unlink(child);
            }

            if (selector.Count == 0)
                ReplaceSelector(tree, parent, selector);

            tree.RemoveNodes(childrenToRemove);
        }

        // Parent -> Selector -> Any = Parent -> Any
        private static bool SimplifySelector(BehaviorTreeAsset tree, IParentNode parent, SelectorNodeAsset selector)
        {
            if (selector.Count != 1)
                return false;

            using var enumerator = selector.Children.GetEnumerator();

            if (!enumerator.MoveNext())
                return false;

            var child = tree.GetNode(enumerator.Current);

            if (child == null)
                return false;

            parent.Replace(selector, child);
            tree.RemoveNodes(new[] { selector });
            return true;
        }

        // Parent -> Selector -> SUCCESS / RUNNING, ... = Parent -> SUCCESS / RUNNING
        private static bool ForceSelectorStatus(BehaviorTreeAsset tree, IParentNode parent, SelectorNodeAsset selector)
        {
            var childrenToRemove = new List<NodeAsset>();
            var removeChildren = false;

            foreach (var child in (selector as IParentNode).GetChildren(tree))
            {
                if (!removeChildren && child is not AlwaysSuccessNodeAsset or AlwaysRunningNodeAsset)
                    continue;

                removeChildren = true;
                childrenToRemove.Add(child);
                selector.Unlink(child);
            }

            if (selector.Count == 0)
                ReplaceSelector(tree, parent, selector);

            tree.RemoveNodes(childrenToRemove);
            return selector.Count == 0;
        }

        // Parent -> Sequence (Empty) = Parent -> SUCCESS
        private static bool EmptySequence(BehaviorTreeAsset tree, IParentNode parent, SequenceNodeAsset sequence)
        {
            if (sequence.Count > 0)
                return false;

            ReplaceSequence(tree, parent, sequence);
            return true;
        }

        // Parent -> Sequence -> SUCCESS, ... = Parent -> Sequence -> ...
        private static void SkipSequenceSuccesses(BehaviorTreeAsset tree, IParentNode parent, SequenceNodeAsset sequence)
        {
            var childrenToRemove = new List<NodeAsset>();

            foreach (var child in (sequence as IParentNode).GetChildren(tree))
            {
                if (child is not AlwaysSuccessNodeAsset)
                    continue;

                childrenToRemove.Add(child);
                sequence.Unlink(child);
            }

            if (sequence.Count == 0)
                ReplaceSequence(tree, parent, sequence);

            tree.RemoveNodes(childrenToRemove);
        }

        // Parent -> Sequence -> Any = Parent -> Any
        private static bool SimplifySequence(BehaviorTreeAsset tree, IParentNode parent, SequenceNodeAsset sequence)
        {
            if (sequence.Count != 1)
                return false;

            using var enumerator = sequence.Children.GetEnumerator();

            if (!enumerator.MoveNext())
                return false;

            var child = tree.GetNode(enumerator.Current);

            if (child == null)
                return false;

            parent.Replace(sequence, child);
            tree.RemoveNodes(new[] { sequence });
            return true;
        }

        // Parent -> Sequence -> FAILURE / RUNNING, ... = Parent -> FAILURE / RUNNING
        private static bool ForceSequenceStatus(BehaviorTreeAsset tree, IParentNode parent, SequenceNodeAsset sequence)
        {
            var childrenToRemove = new List<NodeAsset>();
            var removeChildren = false;

            foreach (var child in (sequence as IParentNode).GetChildren(tree))
            {
                if (!removeChildren && child is not AlwaysFailureNodeAsset or AlwaysRunningNodeAsset)
                    continue;

                removeChildren = true;
                childrenToRemove.Add(child);
                sequence.Unlink(child);
            }

            if (sequence.Count == 0)
                ReplaceSequence(tree, parent, sequence);

            tree.RemoveNodes(childrenToRemove);
            return sequence.Count == 0;
        }

        #endregion

        #region Temporary Nodes

        // ReSharper disable once ClassNeverInstantiated.Local
        private class AlwaysFailureNodeAsset : LeafNodeAsset { }
        
        // ReSharper disable once ClassNeverInstantiated.Local
        private class AlwaysSuccessNodeAsset : LeafNodeAsset { }
        
        // ReSharper disable once ClassNeverInstantiated.Local
        private class AlwaysRunningNodeAsset : LeafNodeAsset { }

        #endregion
    }
}