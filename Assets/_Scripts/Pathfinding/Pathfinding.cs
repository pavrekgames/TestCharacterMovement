using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {

        MapGrid mapGrid; 
        public Transform startPosition; 
        public Transform targetPosition; 

        private void Awake() 
        {
            mapGrid = FindFirstObjectByType<MapGrid>(); 
        }

        private void Update() 
        {
            FindPath(startPosition.position, targetPosition.position); 
        }

        private void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Node startNode = mapGrid.NodeFromWorldPoint(startPos); 
            Node targetNode = mapGrid.NodeFromWorldPoint(targetPos); 

            List<Node> openList = new List<Node>(); 
            HashSet<Node> closedList = new HashSet<Node>(); 

            openList.Add(startNode); 

            while (openList.Count > 0) 
            {
                Node currentNode = openList[0]; 

                for (int i = 1; i < openList.Count; i++) 
                {
                    if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost) 
                    {
                        currentNode = openList[i]; 
                    }
                }
                openList.Remove(currentNode); 
                closedList.Add(currentNode); 

                if (currentNode == targetNode) 
                {
                    GetFinalPath(startNode, targetNode); 
                }

                foreach (Node neighborNode in mapGrid.GetNeighboringNodes(currentNode)) 
                {
                    if (!neighborNode.isWall || closedList.Contains(neighborNode)) 
                    {
                        continue; 
                    }
                    int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighborNode); 

                    if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode)) 
                    {
                        neighborNode.gCost = moveCost; 
                        neighborNode.hCost = GetManhattenDistance(neighborNode, targetNode); 
                        neighborNode.parentNode = currentNode; 

                        if (!openList.Contains(neighborNode)) 
                        {
                            openList.Add(neighborNode); 
                        }
                    }
                }

            }
        }

        private void GetFinalPath(Node startNode, Node endNode)
        {
            List<Node> finalPath = new List<Node>(); 
            Node currentNode = endNode; 

            while (currentNode != startNode) 
            {
                finalPath.Add(currentNode); 
                currentNode = currentNode.parentNode; 
            }

            finalPath.Reverse(); 

            mapGrid.finalPath = finalPath; 

        }

        private int GetManhattenDistance(Node nodeA, Node nodeB)
        {
            int int_X = Mathf.Abs(nodeA.grid_X - nodeB.grid_X); 
            int int_Y = Mathf.Abs(nodeA.grid_Y - nodeB.grid_Y); 

            return int_X + int_Y; 
        }


    }
}


