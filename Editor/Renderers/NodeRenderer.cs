using TreeFlow.Runtime.Core;
using UnityEngine;

namespace TreeFlow.Editor.Renderers
{
    /// <summary>
    /// Class that renders <see cref="VisualizerNode"/>
    /// </summary>
    internal class NodeRenderer
    {
        public const float NODE_WIDTH = 100f;
        public const float NODE_HEIGHT = 50f;
        public const float NODE_MARGIN = 25f;
        public const float NODE_PADDING = 25f;

        private readonly GUIStyle style;

        public NodeRenderer()
        {
            style = new GUIStyle(GUI.skin.box)
            {
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(5, 5, 5, 5)
            };
            
            var bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
            bgTexture.Apply();
            
            style.normal.background = bgTexture;
        }
        
        /// <summary>
        /// Draws the given node onto the screen
        /// </summary>
        public void Draw(VisualizerNode node)
        {
            var originalColor = style.normal.textColor;
            
            style.normal.textColor = GetColor(node);
            
            GUI.Button(
                new Rect(
                    GetScreenX(node.Position.x) + NODE_MARGIN,
                    GetScreenY(node.Position.y) + NODE_MARGIN,
                    NODE_WIDTH,
                    NODE_HEIGHT
                ),
                GetText(node),
                style
            );
            
            style.normal.textColor = originalColor;
        }
        
        public static Color GetColor(VisualizerNode node) => node.Self.Status switch
        {
            NodeStatus.FAILURE => Color.red,
            NodeStatus.SUCCESS => Color.green,
            NodeStatus.RUNNING => Color.yellow,
            _ => Color.gray
        };

        private static string GetText(VisualizerNode node) => node.Self.Alias;

        public static float GetScreenX(float x) => x * (NODE_WIDTH + NODE_PADDING);
        public static float GetScreenY(float y) => y * (NODE_HEIGHT + NODE_PADDING);
    }
}