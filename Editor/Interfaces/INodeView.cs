using TreeFlow.Editor.ScriptableObjects;
using UnityEditor.Experimental.GraphView;

namespace TreeFlow.Editor.Interfaces
{
    /// <summary>
    /// Defines any view that can display a <see cref="NodeAsset"/>
    /// </summary>
    public interface INodeView
    {
        /// <summary>
        /// Sets the title of this node
        /// </summary>
        public void SetTitle(string newTitle);

        /// <summary>
        /// Sets the title to <see cref="defaultTitle"/> if this node has no <see cref="NodeAsset.Name"/> defined
        /// </summary>
        public void SetDefaultTitle(string defaultTitle);

        /// <summary>
        /// Sets the color of the header of this node
        /// </summary>
        public void SetColor(byte r, byte g, byte b);

        /// <summary>
        /// Adds an <see cref="Direction.Input"/> to this node
        /// </summary>
        public void AddInputPort();
        
        /// <summary>
        /// Adds an <see cref="Direction.Output"/> to this node
        /// </summary>
        public void AddOutputPort(bool allowMultiple = false);
    }
}