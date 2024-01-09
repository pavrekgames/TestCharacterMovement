using System.Collections;
using System.Collections.Generic;
using TestCharactersMovement.CharactersSystem;
using UnityEngine;
using static TestCharactersMovement.CharactersSystem.Character;

namespace TestCharactersMovement.SaveLoadSystem
{
    [System.Serializable]
    public class GameData
    {

        public List<CharacterProperties> characters;

        public GameData()
        {
           
            characters = new List<CharacterProperties>();

        }


    }
}


