using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.PathfindingSystem
{
    [Serializable]
    public class Node : IHeapItem<Node>
    {
        public int grid_X; 
        public int grid_Y; 

        public bool isWalkable; 
        public Vector3 worldPosition; 

        public Node parentNode; 
        public int gCost; 
        public int hCost;
        int heapIndex;

        public Node(bool isWall, Vector3 worldPosition, int grid_X, int grid_Y) 
        {
            this.isWalkable = isWall; 
            this.worldPosition = worldPosition; 
            this.grid_X = grid_X; 
            this.grid_Y = grid_Y; 
        }

        public int fCost { get { return gCost + hCost; } }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);

            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            return -compare;
        }


    }
}


