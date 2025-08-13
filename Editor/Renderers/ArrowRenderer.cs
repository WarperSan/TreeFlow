using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.Renderers
{
    /// <summary>
    /// Class that renders arrows
    /// </summary>
    internal class ArrowRenderer
    {
        /// <summary>
        /// Draws an arrow from <see cref="start"/> to <see cref="end"/>
        /// </summary>
        public void Draw(VisualizerNode start, VisualizerNode end)
        {
            var originalColor = Handles.color;
          
            var origin = new Vector2(
                NodeRenderer.GetScreenX(start.Position.x) + NodeRenderer.NODE_WIDTH / 2f + NodeRenderer.NODE_MARGIN,
                NodeRenderer.GetScreenY(start.Position.y) + NodeRenderer.NODE_HEIGHT + NodeRenderer.NODE_MARGIN
            );

            var destination = new Vector2(
                NodeRenderer.GetScreenX(end.Position.x) + NodeRenderer.NODE_WIDTH / 2f + NodeRenderer.NODE_MARGIN,
                NodeRenderer.GetScreenY(end.Position.y) + NodeRenderer.NODE_MARGIN
            );

            Handles.color = end.Color;
            
            var middleY = origin.y + NodeRenderer.NODE_MARGIN / 2f;
            
            Handles.DrawLine(origin, new Vector2(origin.x, middleY));
            Handles.DrawLine(new Vector2(origin.x, middleY), new Vector2(destination.x, middleY));
            Handles.DrawLine(new Vector2(destination.x, middleY), destination);
            
            Handles.color = originalColor;
        }
    }
}