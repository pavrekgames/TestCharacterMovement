using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.CharactersSystem
{
    [CreateAssetMenu(fileName = "Character", menuName = "CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public string characterName;
        public float strength;
        public float magic;
        public float intelligence;
        public float dexterity;
        public CharacterRaces race;

    }
}


