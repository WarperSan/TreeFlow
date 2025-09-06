using System.Collections.Generic;
using TreeFlow.Editor.Helpers;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace TreeFlow.Editor.Elements
{
    /// <summary>
    /// View used to display a tree
    /// </summary>
    internal class TreeView : GraphView
    {
        public TreeView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            AddManipulators();
            CreateBackground();
        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }
        
        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(new ContextualMenuManipulator(menuEvt =>
            {
                const string createMenu = "Create Node";
                
                //menuEvt.menu.AppendAction(createMenu + "/Selector", info => CreateNode("Selector", info.eventInfo));
                //menuEvt.menu.AppendAction(createMenu + "/Sequence", info => CreateNode("Sequence", info.eventInfo));
                //menuEvt.menu.AppendSeparator(createMenu + "/");
                
                //menuEvt.menu.AppendAction(createMenu + "/Inverter", info => CreateNode("Inverter", info.eventInfo));
                //menuEvt.menu.AppendSeparator(createMenu + "/");
                
                //menuEvt.menu.AppendAction(createMenu + "/Callback", info => CreateNode("Callback", info.eventInfo));
            }));
        }

        /// <summary>
        /// Creates the background for the view
        /// </summary>
        private void CreateBackground()
        {
            var styleSheet = ResourceManager.Load<StyleSheet>("5cc0f3e8f1d2465cb9ca4a01f939651b");
            
            if (styleSheet != null)
                styleSheets.Add(styleSheet);
            
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            grid.AddToClassList("gridBackground");
        }

        /*private void CreateNode(string nodeName, DropdownMenuEventInfo info)
        {
            var position = info.mousePosition;

            var node = new NodeView(new SelectorNode())
            {
                title = nodeName,
                //NodeGUID = System.Guid.NewGuid().ToString(),
                style =
                {
                    left = position.x,
                    top = position.y
                }
            };

            node.RefreshExpandedState();
            node.RefreshPorts();

            AddElement(node);
        }*/
    }
}