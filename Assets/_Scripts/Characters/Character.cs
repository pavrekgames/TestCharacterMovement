using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TestCharactersMovement.PathfindingSystem;
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

        [Header("Pathfinding")]
        [SerializeField] private Pathfinding pathfinding;
        [SerializeField] private Vector3[] path;
        [SerializeField] private int targetIndex;
        public Transform target;
        

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

        protected virtual void Initialize()
        {
            pathfinding = FindFirstObjectByType<Pathfinding>();
        }

        protected virtual void Start()
        {
            Initialize();
            SetRandomFactors();

            Pathfinding.OnPathFound += OnPathFound;
        }

        protected void SetRandomFactors()
        {
            speed = Random.Range(1, 3);
            agility = Random.Range(1, 4);
            resistance = Random.Range(1, 5);

            Debug.Log("RandomFactors");
        }

        public virtual void Move(Vector3 target)
        {
            pathfinding.FindPath(transform.localPosition, target);
        }

        public virtual void Follow()
        {

        }

        public void OnPathFound(Vector3[] newPath, bool isPathSuccessful)
        {
            if (isPathSuccessful)
            {
                path = newPath;
                targetIndex = 0;

                if (isSelected)
                {
                    StopCoroutine(FollowPath());
                    StartCoroutine(FollowPath());
                }
                
            }
        }

        private IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentWaypoint, speed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(currentWaypoint - transform.localPosition), agility * Time.deltaTime);
                yield return null;
            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }

        #region Selecting

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

        #endregion

        #region Savings 

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

        #endregion

    }
}


