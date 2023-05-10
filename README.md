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


# Minor Changes
We have added three new features: a goat, a boat, and a pickaxe. As the player travels through the map, they will encounter these features, which will change their interactions with the environment.

### Goat
The player will have the ability to climb mountains.

### Boat
The player will have the ability to sail in sea.

### Pick Axe
The player will be able to turn mountains into grass.



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

