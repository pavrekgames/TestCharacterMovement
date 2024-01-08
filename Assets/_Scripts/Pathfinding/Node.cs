using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.Pathfinding
{
    [Serializable]
    public class Node
    {
        public int grid_X; 
        public int grid_Y; 

        public bool isWall; 
        public Vector3 position; 

        public Node parentNode; 

        public int gCost; 
        public int hCost; 

        public int fCost { get { return gCost + hCost; } } 

        public Node(bool isWall, Vector3 position, int grid_X, int grid_Y) 
        {
            this.isWall = isWall; 
            this.position = position; 
            this.grid_X = grid_X; 
            this.grid_Y = grid_Y; 
        }

    }
}


