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

        #region Ports
        
        /// <summary>
        /// <see cref="Direction.Input"/> port of this node
        /// </summary>
        protected Port InputPort { get; }
        
        /// <summary>
        /// <see cref="Direction.Output"/> port of this node
        /// </summary>
        protected Port OutputPort { get; }

        /// <summary>
        /// Adds an <see cref="Direction.Input"/> to this node
        /// </summary>
        public void AddInputPort();
        
        /// <summary>
        /// Adds an <see cref="Direction.Output"/> to this node
        /// </summary>
        public void AddOutputPort(bool allowMultiple = false);

        /// <summary>
        /// Connects the output port of the given parent to the input port of this node
        /// </summary>
        public Edge ConnectTo(INodeView parent) => parent.OutputPort?.ConnectTo(InputPort);

        #endregion
    }
}