using System.Collections;
using System.Collections.Generic;
using TestCharactersMovement.SaveLoadSystem;
using UnityEngine;

namespace TestCharactersMovement.CharactersSystem
{
    
    public abstract class Character : MonoBehaviour, ISaveLoadData
    {
        [Header("General")]
        public CharacterHUD characterHUD;
        public bool isSelected = false;

        [Header("Character Properties")]
        public CharacterData characterData;
        public float speed;
        public float agility;
        public float resistance;
        public Vector3 position;
        public Quaternion rotation;

        [Header("Savings Data")]
        [HideInInspector] public CharacterProperties characterProperties;

        [System.Serializable]
        public class CharacterProperties
        {
            public CharacterData characterData;
            public float speed;
            public float agility;
            public float resistance;
            public Vector3 position;
            public Quaternion rotation;
        }

        protected virtual void Start()
        {
            SetRandomFactors();
        }

        protected void SetRandomFactors()
        {
            speed = Random.Range(1, 5);
            agility = Random.Range(1, 8);
            resistance = Random.Range(1, 5);

            Debug.Log("RandomFactors");
        }

        public virtual void Move(Vector3 target)
        {

        }

        public virtual void Follow()
        {

        }

        public void Select()
        {
            characterHUD.SetCharacterSelected();
            isSelected = true;

            Debug.Log(characterProperties.characterData.characterName);
        }

        public void Deselect()
        {
            characterHUD.SetCharacterDeselected();
            isSelected = false;
        }

        public void Save(GameData gameData)
        {
            characterProperties.speed = speed;
            characterProperties.agility = agility;
            characterProperties.resistance = resistance;
            characterProperties.position = transform.localPosition;
            characterProperties.rotation = transform.localRotation;

            CharacterProperties savingCharacter = gameData.characters.Find(x => x.characterData.characterName == characterProperties.characterData.characterName);

            if (savingCharacter != null)
            {
                if (savingCharacter.characterData.characterName == characterProperties.characterData.characterName)
                {
                    gameData.characters.Remove(savingCharacter);
                    gameData.characters.Add(characterProperties);
                }
            }
            else
            {
                gameData.characters.Add(characterProperties);
            }
        }

        public void Load(GameData gameData)
        {

            CharacterProperties loadedCharacter = gameData.characters.Find(x => x.characterData.characterName == characterProperties.characterData.characterName);

            speed = loadedCharacter.speed;
            agility = loadedCharacter.agility;
            resistance = loadedCharacter.resistance;
            position = loadedCharacter.position;
            rotation = loadedCharacter.rotation;

            transform.localPosition = loadedCharacter.position;
            transform.localRotation = loadedCharacter.rotation;

        }
    }
}


