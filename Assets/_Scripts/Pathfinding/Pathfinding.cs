using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.PathfindingSystem
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private MapGrid mapGrid;
        [SerializeField] private bool pathSuccess = false;

        public delegate void PathDelegate(Vector3[] wayPoints, bool isPathSuccess);
        public static PathDelegate OnPathFound;

        private void Awake()
        {
            mapGrid = FindFirstObjectByType<MapGrid>();
        }

        public void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            StartCoroutine(FindPath_IE(startPos, targetPos));
        }

        private IEnumerator FindPath_IE(Vector3 startPos, Vector3 targetPos)
        {
            Vector3[] waypoints = new Vector3[0];
            pathSuccess = false;

            Node startNode = mapGrid.NodeFromWorldPoint(startPos);
            Node targetNode = mapGrid.NodeFromWorldPoint(targetPos);

            if (startNode.isWalkable && targetNode.isWalkable)
            {
                Heap<Node> openSet = new Heap<Node>(mapGrid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in mapGrid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parentNode = currentNode;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                        }
                    }
                }
            }
            yield return null;

            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, targetNode);
            }

            OnPathFound?.Invoke(waypoints, pathSuccess);
        }

        private Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parentNode;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            return waypoints;
        }

        private Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].grid_X - path[i].grid_X, path[i - 1].grid_Y - path[i].grid_Y);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i - 1].worldPosition);
                }
                directionOld = directionNew;
            }

            return waypoints.ToArray();
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.grid_X - nodeB.grid_X);
            int dstY = Mathf.Abs(nodeA.grid_Y - nodeB.grid_Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

    }
}


