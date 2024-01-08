using System.Collections;
using System.Collections.Generic;
using TestCharactersMovement.SaveLoadSystem;
using UnityEngine;

namespace TestCharactersMovement.CharactersSystem
{
    public abstract class Character : MonoBehaviour, ISaveLoadData
    {
        [Header("General")]
        [SerializeField] protected CharacterData characterData;
        public CharacterHUD characterHUD;
        public bool isSelected = false;

        [Header("Properties")]
        public float speed;
        public float agility;
        public float resistance;
        public Vector3 position;
        public Quaternion rotation;


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

            Debug.Log(characterData.characterName);
        }

        public void Deselect()
        {
            characterHUD.SetCharacterDeselected();
            isSelected = false;
        }

        public void Save(GameData gameData)
        {
            if (gameData.characters.Contains(this))
            {
                gameData.characters.Remove(this);
            }

            gameData.characters.Add(this);

        }

        public void Load(GameData gameData)
        {

            Character loadedCharacter = gameData.characters.Find(x => x.characterData == this.characterData);

            speed = loadedCharacter.speed;
            agility = loadedCharacter.agility;
            resistance = loadedCharacter.resistance;
            position = loadedCharacter.position;
            rotation = loadedCharacter.rotation;

            transform.localPosition = position;
            transform.localRotation = rotation;

        }
    }
}


