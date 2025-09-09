using UnityEditor.Experimental.GraphView;

namespace TreeFlow.Editor.UIElements
{
    /// <summary>
    /// Element used to display and modify <see cref="GraphNode"/>
    /// </summary>
    public sealed class NodeView : Node
    {
        /// <summary>
        /// Current node displayed
        /// </summary>
        public GraphNode Node { get; private set; }
        
        /// <summary>
        /// Assigns the node to display to the given <see cref="GraphNode"/>
        /// </summary>
        /// <param name="node"></param>
        public void AssignNode(GraphNode node)
        {
            var rect = GetPosition();
            rect.position = node.Position;
            SetPosition(rect);
            
            Node = node;
        }
    }
}