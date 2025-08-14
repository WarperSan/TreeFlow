# Tree Flow
**Tree Flow** is a lightweight and modular framework for **[Behaviour Trees](https://en.wikipedia.org/wiki/Behavior_tree_(artificial_intelligence,_robotics_and_control))**. It provides an easy-to-use model to quickly create, debug and release new AI patterns.

## Features
- Easy and modular framework to create behaviour trees
- Built-in nodes like selector, sequence and inverter
- Supports custom nodes, using given classes or from scratch
- Built-in visualizer to watch the tree in action

## Usage
### Create a new node
You might need a node that will be used in several places. This package allows to easily create composite nodes, decorator nodes, leaf nodes or any type of nodes:

```csharp
using TreeFlow.Runtime.Core;

class NewNode : BaseComposite
{
    // ...
}

// BaseComposite for Composite / Control nodes
// BaseDecorator for Decorator nodes
// BaseLeaf for Leaf nodes
// BaseNode for specific nodes
// INode for nodes made from scratch
```

### Using Callback Node
You might need a node for a very specific one-time functionality. This package offers a built-in solution that is easy to use for a *throwaway* node:

```csharp
new CallbackNode(node =>
{
    // ...

    return NodeStatus.SUCCESS;
}
```

You can use a lambda to create a node, removing the need to create unnecessary nodes.

## Tree Visualizer
The visualizer is a useful tool that allows you to see your tree in action.

To open it, you need to go to `Window -> Tree Visualizer`, This will open a window that will show you the current tree inspected. You will see the structure of the tree and their different state.

## Contributing
Contributions, bug reports, and feature requests are welcome! Please submit pull requests or open issues on the repository.

## License
Project under the MIT License