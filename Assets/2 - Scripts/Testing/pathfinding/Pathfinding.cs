using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public Node GetRandomNodeFromGrid(bool walkable) {
        Node randomNode = grid.GetNodeFromGrid(UnityEngine.Random.Range(0, grid.gridSizeX), UnityEngine.Random.Range(0, grid.gridSizeY), UnityEngine.Random.Range(0, grid.gridSizeZ));
        if (randomNode.walkable == walkable) {
            return randomNode;
        }
        else {
            while (randomNode.walkable != walkable) { 
                randomNode = grid.GetNodeFromGrid(UnityEngine.Random.Range(0, grid.gridSizeX), UnityEngine.Random.Range(0, grid.gridSizeY), UnityEngine.Random.Range(0, grid.gridSizeZ));
            }
        }

        return randomNode;
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable) {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while(openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    // int newMovementCostToNeighbor = currentNode.gCost + (int)(new Vector3(currentNode.gridX, currentNode.gridY, currentNode.gridZ) - new Vector3(neighbor.gridX, neighbor.gridY, neighbor.gridZ)).magnitude;
                    // int newMovementCostToNeighbor = currentNode.gCost + (int)(currentNode.worldPosition - neighbor.worldPosition).magnitude;
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        // neighbor.hCost = currentNode.gCost + (int)(new Vector3(currentNode.gridX, currentNode.gridY, currentNode.gridZ) - new Vector3(targetNode.gridX, targetNode.gridY, targetNode.gridZ)).magnitude;
                        // neighbor.hCost = currentNode.gCost + (int)(currentNode.worldPosition - targetNode.worldPosition).magnitude;
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector3 directionNew = new Vector3(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY, path[i - 1].gridZ - path[i].gridZ);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].worldPosition);
                directionOld = directionNew;
            }
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int distZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        // if (distX > distY)
        // {
        //    return 14 * distY + 10 * (distX - distY);
        // }
        // return 14 * distX + 10 * (distY - distX);

        return distX + distY + distZ;
    }
}