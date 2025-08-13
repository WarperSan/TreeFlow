using System.Collections.Generic;
using TreeFlow.Runtime.Core;
using UnityEngine;

namespace TreeFlow.Editor.Renderers
{
    /// <summary>
    /// Class that renders an entire tree
    /// </summary>
    internal class TreeRenderer
    {
        private readonly ArrowRenderer arrowRenderer = new();
        private readonly NodeRenderer nodeRenderer = new();

        /// <summary>
        /// Draws the tree from the given root onto the screen
        /// </summary>
        public void Draw(BaseNode root)
        {
            var visualizerNode = new VisualizerNode(root);
            var treeSize = VisualizerNode.ArrangeNodes(visualizerNode, Vector2.zero);
         
            GUILayout.Box(
                new GUIContent(),
                GUILayout.Width(treeSize.x),
                GUILayout.Height(treeSize.y)
            );


            var stack = new Stack<VisualizerNode>();
            stack.Push(visualizerNode);
            
            nodeRenderer.Draw(visualizerNode);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                
                foreach (var child in current.Children)
                {
                    nodeRenderer.Draw(child);
                    arrowRenderer.Draw(current, child);
                    
                    if (child.Self is BaseComposite or BaseDecorator)
                        stack.Push(child);
                }
            }
        }
    }
}