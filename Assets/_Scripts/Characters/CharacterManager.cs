using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCharactersMovement.CharactersSystem
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Characters")]
        [SerializeField] private List<Character> allCharacters = new List<Character>();
        [SerializeField] private Character currentCharacter;

        [Header("Character Target")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector3 screenPosition;
        [SerializeField] private Vector3 target;
        [SerializeField] private LayerMask layerMask;

        private void Initialize()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            Initialize();

            CharacterHUD.OnCharacterSelected += SelectCharacter;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetTargetAndTryMove();
            }
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
                character.Deselect();
            }
        }

        private void SetTargetAndTryMove()
        {
            if (currentCharacter != null)
            {
                screenPosition = Input.mousePosition;

                Ray ray = mainCamera.ScreenPointToRay(screenPosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
                {
                    target = hitInfo.point;
                    currentCharacter.FindPathToTarget(target);
                }
            }
        }

    }
}


