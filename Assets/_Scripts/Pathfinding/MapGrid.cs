using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.Pathfinding
{
    public class MapGrid : MonoBehaviour
    {

        [SerializeField] private Transform map;

        public Transform startPosition; 
        public LayerMask wallMask; 
        public Vector2 gridWorldSize; 
        public float nodeRadius; 
        public float distanceBetweenNodes;

        Node[,] nodesArray; 
        public List<Node> finalPath; 

        float nodeDiameter; 
        int gridSizeX, gridSizeY; 


        private void Start() 
        {
            nodeDiameter = nodeRadius * 2; 
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); 
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); 
            CreateGrid(); 
        }

        private void CreateGrid()
        {
            nodesArray = new Node[gridSizeX, gridSizeY]; 
            Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++) 
            {
                for (int y = 0; y < gridSizeY; y++) 
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); 
                    bool Wall = true; 

                    if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                    {
                        Wall = false;
                    }

                    nodesArray[x, y] = new Node(Wall, worldPoint, x, y);
                }
            }
        }

        public List<Node> GetNeighboringNodes(Node neighborNode)
        {
            List<Node> neighborList = new List<Node>(); 
            int check_X; 
            int check_Y; 

            check_X = neighborNode.grid_X + 1;
            check_Y = neighborNode.grid_Y;

            if (check_X >= 0 && check_X < gridSizeX) 
            {
                if (check_Y >= 0 && check_Y < gridSizeY) 
                {
                    neighborList.Add(nodesArray[check_X, check_Y]); 
                }
            }
            
            check_X = neighborNode.grid_X - 1;
            check_Y = neighborNode.grid_Y;

            if (check_X >= 0 && check_X < gridSizeX) 
            {
                if (check_Y >= 0 && check_Y < gridSizeY) 
                {
                    neighborList.Add(nodesArray[check_X, check_Y]); 
                }
            }
            
            check_X = neighborNode.grid_X;
            check_Y = neighborNode.grid_Y + 1;

            if (check_X >= 0 && check_X < gridSizeX) 
            {
                if (check_Y >= 0 && check_Y < gridSizeY) 
                {
                    neighborList.Add(nodesArray[check_X, check_Y]); 
                }
            }
            
            check_X = neighborNode.grid_X;
            check_Y = neighborNode.grid_Y - 1;

            if (check_X >= 0 && check_X < gridSizeX) 
            {
                if (check_Y >= 0 && check_Y < gridSizeY) 
                {
                    neighborList.Add(nodesArray[check_X, check_Y]); 
                }
            }

            return neighborList;
        }

        public Node NodeFromWorldPoint(Vector3 worldPos)
        {
            float pos_X = ((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
            float pos_Y = ((worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

            pos_X = Mathf.Clamp01(pos_X);
            pos_Y = Mathf.Clamp01(pos_Y);

            int intPos_X = Mathf.RoundToInt((gridSizeX - 1) * pos_X);
            int intPos_Y = Mathf.RoundToInt((gridSizeY - 1) * pos_Y);

            return nodesArray[intPos_X, intPos_Y];
        }


        private void OnDrawGizmos()
        {

            Gizmos.DrawWireCube(map.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); 

            if (nodesArray != null) 
            {
                foreach (Node node in nodesArray) 
                {
                    if (node.isWall) 
                    {
                        Gizmos.color = Color.white; 
                    }
                    else
                    {
                        Gizmos.color = Color.yellow; 
                    }


                    if (finalPath != null) 
                    {
                        if (finalPath.Contains(node)) 
                        {
                            Gizmos.color = Color.red; 
                        }

                    }


                    Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - distanceBetweenNodes)); 
                }
            }
        }


    }
}


