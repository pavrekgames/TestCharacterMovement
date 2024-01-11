using System.Collections;
using System.Collections.Generic;
using TestCharactersMovement.Addressables;
using UnityEngine;

namespace TestCharactersMovement.PathfindingSystem
{
    public class MapGrid : MonoBehaviour
    {

        public bool displayGridGizmos;
        public LayerMask unwalkableMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        public float distanceBetweenNodes;

        Node[,] grid;

        float nodeDiameter;
        int gridSizeX, gridSizeY;

        private void Start()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

            SpawnAddressableAssets.OnObjectsSpawned += CreateGrid;
            //CreateGrid();
        }

        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.grid_X + x;
                    int checkY = node.grid_Y + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 worldPos)
        {
            float pos_X = ((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
            float pos_Y = ((worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

            pos_X = Mathf.Clamp01(pos_X);
            pos_Y = Mathf.Clamp01(pos_Y);

            int intPos_X = Mathf.RoundToInt((gridSizeX) * pos_X);
            int intPos_Y = Mathf.RoundToInt((gridSizeY) * pos_Y);

            return grid[intPos_X + 1, intPos_Y + 1];
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null && displayGridGizmos)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = (node.isWalkable) ? Color.white : Color.yellow;
                    Gizmos.DrawCube(node.worldPosition, new Vector3(1, 0.1f, 1));
                }
            }
        }

    }
}


