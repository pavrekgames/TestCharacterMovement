using System.Collections;
using System.Collections.Generic;
using TestCharactersMovement.CharactersSystem;
using UnityEngine;

namespace TestCharactersMovement.SaveLoadSystem
{
    [System.Serializable]
    public class GameData
    {

        public List<Character> characters;

        public GameData()
        {
           
            characters = new List<Character>();

        }


    }
}


