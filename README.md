# Unity Assets

## Bonsai Behaviour Tree 
Advanced behavior tree solution with a graphical editor

![Bonsai Logo](http://i.imgur.com/rq9Tfja.png)

[Quick video showing usage of the editor](https://twitter.com/Unit_978/status/870426657781186560)

Goals of the project
- Lightweight, robust, and fast behaviour trees.
- Visual editor to improve the workflow when creating, running, and testing behaviour trees.
- Seamless integration with the Unity workflow/environment.

Features Overview:

- The core behaviour tree engine with a set of standard composite, decorator, and task nodes.
- Blackboard to share data between tasks.
- A visual editor to create, edit, view, and debug trees
- Conditional Aborts (AKA Observer aborts)
- Includes: Parallel execution, Interrupts, Semaphore Guards, Reactive (Dynamic) Selectors.
- Supports including Sub-trees
- Can easily create custom Composites, Decorators, Tasks
- Behaviour trees are ScriptableObjects, so it integrates perfectly with the Unity Editor.

Behaviour tree running.

![Behaviour tree running](http://i.imgur.com/aUe8neD.png)

### Run-time Editor Features and Limitations

During run-time you can view how the a tree executes and see which nodes are running, the statuses returned (success/failure) or if the nodes were aborted/interrupted.

You can also edit certain properties of a node, like changing the abort type, or setting a new waiting time for the wait task via the Unity Inspector.

Things that cannot be currently edited in run-time are (this may change in the future):
- Adding/deleting nodes
- Changing the root
- Changing connections between nodes

### Editor Features

- A canvas which can be panned and zoomed
- Add, delete, drag, duplicate, and connect nodes.
- There is multi selection support so you can apply multi-edit/drag/duplicate/delete.
- Grid snapping
- Sub-tree dragging - when you drag a node, the entire sub-tree under it drags along.
- Save and load behaviour tree assets.
- Nodes resize properly to fit internal contents such as the name of node.
- Context menus to organize nodes.
- Attributes which can be used on a custom node class to categorize and add an icon to your custom node.
- A custom blackboard inspector to add variables (specify key and type).
- Custom inspector for nodes that need to reference other nodes like Interrupts and Semaphore Guards, the inspector lets you push a button to activate a linking action, in which you can click on nodes to link.
- A simple "nicify" feature which automatically lays out the tree neatly.
- Visual highlighting feedback to quickly see what nodes are being referenced by other nodes and which nodes get aborted by a conditional abort.
- Multiple behaviour tree editors can be opened at once.
- Viewing a running behaviour tree just requires clicking on a game object with behaviour tree component.
- Behaviour tree assets can be opened by double clicking on the asset file.
- The UNDO is still not implemented.

### API and Custom Tasks
There are four main categories of nodes which you can extend from to add functionality:

- Composite
- Decorator
- Conditional Abort
- Conditional Task
- Task

In order to add custom functionality you can override key methods:
 ## Autonomous Agents
 Collection of steering behaviors
