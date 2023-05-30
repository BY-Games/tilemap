# Tilemap

This project is an additional improvement for the game found at
[pathfinding tilemap](https://github.com/gamedev-at-ariel/05-tilemap-pathfinding)

# Try our additional improvement
1. [Minor changes](https://by-games.itch.io/tilemap-game)
2. [Algorithm](https://by-games.itch.io/tilemap-game-astar)

# About the game:
The game is a 2D world represented by a tile graph. Each tile can be one of the following terrain types:
* bushes
* Bushes
* Grass
* Hills
* Swamp
* Sea
* Mountains
* Forest

The player is allowed to walk on the following terrain types:

* Bushes
* Grass
* Hills
* Swamp


# Chasing and Catching Agent in 2D Tilemap
This project implements an agent that learns to chase and catch another object within a 2D tilemap environment.
The agent's goal is to capture the target object by moving towards it.

## Environment Setup
The agent and target object are spawned in random positions within the tilemap at the beginning of each episode. The tilemap consists of different types of tiles, and not all tiles are suitable for both the agent and the target object to be on.

Demo can be found in scene "ML-Agent" and can be tested in Unity.

## Training the Agent
The agent is trained using the Unity Machine Learning Agents (ML-Agents) framework.

During training, the agent learns to navigate the tilemap and catch the target object by optimizing its behavior through trial and error. The agent receives rewards or penalties based on its actions and their consequences, allowing it to learn the optimal strategy for capturing the target object.
To ensure that the agent and target object are placed on valid tiles, the environment uses a position generation function that checks if the generated position is on an allowed tile for both entities. If the generated position is not suitable, a new position is generated until a valid one is found.

You can refer to [ChaseThePlayerAgent.cs](Assets%2FScripts%2FML-Agent%2FChaseThePlayerAgent.cs) script for more details on the implementation.
## Model
The trained agent's model has been exported to the ONNX format and saved as [ChaseThePlayer.onnx](Assets%2FModels%2FChaseThePlayer.onnx).

![Stats.png](Stats.png)


To use the ONNX model, you will need to have the necessary libraries or frameworks installed that support loading and running ONNX models.
See more information about requirements and installations at [ml-agents](https://github.com/Unity-Technologies/ml-agents).

# Minor Changes
We have added three new features: a goat, a boat, and a pickaxe. As the player travels through the map, they will encounter these features, which will change their interactions with the environment.

### Goat
The player will have the ability to climb mountains.

### Boat
The player will have the ability to sail in sea.

### Pick Axe
The player will be able to turn mountains into grass.

In [KeyboardMoverByTile.cs](Assets%2FScripts%2F2-player%2FKeyboardMoverByTile.cs), we have added an OnTrigger method to detect interactions between the player and the items. This is handled using a switch case.
When the player picks up an item, it will be adjusted in [AllowedTiles.cs](Assets%2FScripts%2F1-tiles%2FAllowedTiles.cs) for each item, to be allowed with the special tiles associated with that item.
For the pickaxe, each allowed tile for mountain climbing will be changed to grass using SetTile().



# Algorithm 

Instead of using the BFS algorithm to find the shortest path, we implemented the A* algorithm to find the shortest path with weights.

Our first changes were made by calling the method [MakeOneStepTowardsTheTarget()](https://github.com/BY-Games/tilemap/blame/main/Assets/Scripts/2-player/TargetMover.cs#:~:text=void-,MakeOneStepTowardsTheTarget,-())  in the script [TargetMover.cs](https://github.com/BY-Games/tilemap/blame/main/Assets/Scripts/2-player/TargetMover.cs).

The A* algorithm is implemented in the script [AStar.cs](https://github.com/BY-Games/tilemap/blob/main/Assets/Scripts/5-A-Star/AStar.cs)  and contains the following:


[PriorityQueue<T>](https://github.com/BY-Games/tilemap/blame/main/Assets/Scripts/5-A-Star/AStar.cs#:~:text=%7B-,public,%3E,-%7B) 
This class defines a priority queue that is used to keep track of nodes and their distance from the start node. The priority queue is implemented as a list of tuples, where the first element of the tuple is the node and the second element is the distance from the start node.


[H(Vector3Int node, Vector3Int goal)](https://github.com/BY-Games/tilemap/blame/main/Assets/Scripts/5-A-Star/AStar.cs#:~:text=float-,H,-()) 
This method calculates the estimated distance between two points in the tile map (current node to target node) using the Vector3Int.Distance() method.

[Cost(string tile)](https://github.com/BY-Games/tilemap/blame/main/Assets/Scripts/5-A-Star/AStar.cs#:~:text=float-,Cost,-())
This method calculates the cost of moving from one tile to another in the tile map. The cost of moving from one tile to another is determined by the type of tile. The default cost for grass tiles is 1. Bush tiles are slower than grass tiles, and hill tiles are slower than bush tiles. The method returns the cost of moving from one tile to another.

[FindPath(IGraph<Vector3Int> graph, Vector3Int startNode, Vector3Int endNode, Tilemap tileMap)](https://github.com/BY-Games/tilemap/blame/main/Assets/Scripts/5-A-Star/AStar.cs#:~:text=%3E-,FindPath,-())
This method is the main method that implements the A* algorithm. It takes in the tile map graph, start node, end node, and the tile map as parameters. The method uses the priority queue and cost functions to find the shortest path between the start and end nodes in the tilemap.

The method first creates a priority queue to hold nodes and their distance from the start node. It then creates dictionaries to keep track of the path and cost of each node visited.

The method then enters a while loop that continues until the priority queue is empty or the target node is found. The method dequeues the node with the lowest priority (i.e., the node with the lowest cost to reach it from the start node). It then checks each neighbor of the current node to find the best path to the target.

For each neighbor, the method calculates the cost of moving from the current node to the neighbor using the Cost() method. If the neighbor has not yet been visited or the newly calculated cost to reach it is lower than the previously calculated cost, the method updates the cost to reach the neighbor and adds it to the queue with its new cost as priority.

If the target node is not found, the method returns an empty list. If the target node is found, the method creates a list of vectors representing the path from the start node to the end node and returns it.

