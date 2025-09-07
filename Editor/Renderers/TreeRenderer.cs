using System.Collections.Generic;
using TreeFlow.Runtime.Core;
using UnityEditor;
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

        private VisualizerNode _root;
        private Vector2 treeSize;
        
        /// <summary>
        /// Sets the current tree to the given tree
        /// </summary>
        public void SetTree(Node root)
        {
            if (root is null)
            {
                _root = null;
                treeSize = Vector2.zero;
                return;
            }
            
            _root = new VisualizerNode(root);
            treeSize = VisualizerNode.ArrangeNodes(_root, Vector2.zero);
        }

        /// <summary>
        /// Draws the current tree onto the screen
        /// </summary>
        public void Draw()
        {
            if (_root is null)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("No tree selected", EditorStyles.centeredGreyMiniLabel);
                GUILayout.FlexibleSpace();
                return;
            }

            GUILayout.Box(
                new GUIContent(),
                GUILayout.Width(treeSize.x),
                GUILayout.Height(treeSize.y)
            );

            var stack = new Stack<VisualizerNode>();
            stack.Push(_root);
            
            nodeRenderer.Draw(_root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                
                foreach (var child in current.Children)
                {
                    nodeRenderer.Draw(child);
                    arrowRenderer.Draw(current, child);
                    
                    if (child.Self is Composite or Decorator)
                        stack.Push(child);
                }
            }
        }
    }
}