using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class AStar
{

    public class PriorityQueue<T>
    {
        private List<(T, float)> elements = new List<(T, float)>();

        public int Count => elements.Count;

        public void Enqueue(T item, float priority)
        {
            elements.Add((item, priority));
        }

        public (T, float) Dequeue()
        {
            int bestIndex = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            var bestItem = elements[bestIndex];
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }



    /*
     * Calculates the estimated distance between two points in the tile map (current node to target node).
     */
    public static float H(Vector3Int node, Vector3Int goal)
    {
        float distance = Vector3Int.Distance(node, goal);
        return distance;

    }

    /**
     * This method is used to calculate the cost of moving from one tile to another in the tile map. 
     */
    public static float Cost(string tile)
    {
        float cost = 1f; // default cost for grass tiles

        if (tile == "bushes")
        {

            cost = 2f; // bush tiles are slower than grass tiles hill tiles
        }
        else if (tile == "hill")
        {
            cost = 3f; // hill tiles are slower than hill tiles
        }


        return cost;
    }

    //A star algorithm
    public static List<Vector3Int> FindPath(
        IGraph<Vector3Int> graph,
        Vector3Int startNode,
        Vector3Int endNode,
        Tilemap tileMap
    )
    {
        

        // Create a priority queue to hold nodes and their distance from the start node
        var distanceQueue = new PriorityQueue<Vector3Int>();
        distanceQueue.Enqueue(startNode, 0);

        var camefrom = new Dictionary<Vector3Int, Vector3Int>();//track of the path
        var costSoFar = new Dictionary<Vector3Int, float>(); //cost of each node visited

        camefrom[startNode] = startNode;
        costSoFar[startNode] = 0;

        while (distanceQueue.Count > 0)
        {
            var current = distanceQueue.Dequeue();


            // If the current node is the target node, we can stop searching
            if (current.Equals(endNode))
            {
                break;
            }

            // Check each neighbor of the current node to find the best path to the target
            foreach (var neighbor in graph.Neighbors(current.Item1))
            {
                // Calculate the cost of moving from the current node to this neighbor.
                float G = costSoFar[current.Item1] + Cost(tileMap.GetTile(neighbor).name);

                if (!costSoFar.ContainsKey(neighbor) || G < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = G;


                    float F = G + H(neighbor, endNode);
                    distanceQueue.Enqueue(neighbor, F);
                    camefrom[neighbor] = current.Item1;
                }
            }
        }
        //target was not found
        if (!camefrom.ContainsKey(endNode))
        {
            return new List<Vector3Int>();
        }

        var path = new List<Vector3Int>();
        var current2 = endNode;
        while (!current2.Equals(startNode))
        {
            path.Add(current2);
            current2 = camefrom[current2];
        }

        path.Add(startNode);
        path.Reverse();
        return path;
    }
}
