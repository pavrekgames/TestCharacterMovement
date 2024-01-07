using System.Collections;
using System.Collections.Generic;
using TestCharactersMovement.CharactersSystem;
using UnityEngine;

namespace TestCharactersMovement.CharactersSystem
{
    public abstract class Character : MonoBehaviour
    {
        protected CharacterData characterData;
        public bool isSelected = false;
        public float speed;
        public float agility;
        public float resistance;
        public Vector3 position;
        public Quaternion rotation;


        protected void SetRandomFactors()
        {

        }

        public virtual void Move(Vector3 target)
        {

        }

        public virtual void Follow()
        {

        }

        public void Select()
        {

        }

        public void Deselect()
        {

        } 


    }
}


