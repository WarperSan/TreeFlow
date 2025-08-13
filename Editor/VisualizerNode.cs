using System;
using System.Collections.Generic;
using TreeFlow.Editor.Renderers;
using TreeFlow.Runtime.Core;
using UnityEngine;

namespace TreeFlow.Editor
{
    /// <summary>
    /// Node used to display <see cref="BaseNode"/> in the tree visualizer
    /// </summary>
    internal class VisualizerNode
    {
        public Vector2 Position;
        public readonly BaseNode Self;
        public readonly List<VisualizerNode> Children = new();

        public Color Color => Self.Status switch
        {
            NodeStatus.FAILURE => Color.red,
            NodeStatus.SUCCESS => Color.green,
            NodeStatus.RUNNING => Color.yellow,
            _ => Color.gray
        };

        public VisualizerNode(BaseNode node)
        {
            Self = node;

            switch (node)
            {
                case BaseComposite composite:
                {
                    foreach (var child in composite)
                        Children.Add(new VisualizerNode(child));
                    break;
                }
                case BaseDecorator decorator:
                    Children.Add(new VisualizerNode(decorator.GetChild()));
                    break;
            }
        }

        /// <summary>
        /// Arranges the given node and its children from the given origin
        /// </summary>
        public static Vector2 ArrangeNodes(VisualizerNode node, Vector2 origin)
        {
            var minPos = Vector2.zero;
            var maxPos = Vector2.zero;
            
            node.CalculatePosition(origin.x, origin.y, ref minPos, ref maxPos);

            minPos.x = NodeRenderer.GetScreenX(minPos.x);
            minPos.y = NodeRenderer.GetScreenY(minPos.y);
            maxPos.x = NodeRenderer.GetScreenX(maxPos.x + 1);
            maxPos.y = NodeRenderer.GetScreenY(maxPos.y + 1);
            
            return new Vector2(
                maxPos.x - minPos.x,
                maxPos.y - minPos.y
            );
        }
        
        /// <summary>
        /// Calculates the position of this node based of the given origin
        /// </summary>
        private float CalculatePosition(float x, float y, ref Vector2 min, ref Vector2 max)
        {
            var offset = 0f;

            // Calculate positions for children
            foreach (var node in Children)
                offset += node.CalculatePosition(x + offset, y + 1, ref min, ref max);

            offset = Math.Max(offset, 1);

            Position.x = x + (offset - 1) / 2f;
            Position.y = y;

            min.x = Mathf.Min(min.x, Position.x);
            min.y = Mathf.Min(min.y, Position.y);
            max.x = Mathf.Max(max.x, Position.x);
            max.y = Mathf.Max(max.y, Position.y);

            return offset;
        }
    }
}