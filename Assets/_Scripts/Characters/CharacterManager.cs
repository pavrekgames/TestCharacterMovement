using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.CharactersSystem
{
    public class CharacterManager : MonoBehaviour
    {

        [SerializeField] private List<Character> allCharacters = new List<Character>();
        [SerializeField] private Character currentCharacter;

        private void Start ()
        {
            CharacterHUD.OnCharacterSelected += SelectCharacter;
        }

        public void SelectCharacter(Character character)
        {
            currentCharacter = character;
            DeselectAllCharacters();
            currentCharacter.Select();
        }

        private void DeselectAllCharacters()
        {
            foreach (Character character in allCharacters)
            {
                character.characterHUD.SetCharacterDeselected();
            }
        }

        private void SetTarget()
        {

        }

        private void MoveCharacter()
        {

        }


    }
}


