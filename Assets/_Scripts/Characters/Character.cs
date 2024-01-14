using System;
using System.Collections;
using System.Collections.Generic;
using TestCharactersMovement.PathfindingSystem;
using TestCharactersMovement.SaveLoadSystem;
using UnityEngine;

namespace TestCharactersMovement.CharactersSystem
{
    public abstract class Character : MonoBehaviour, ISaveLoadData
    {
        [Header("General")]
        [SerializeField] private Animator animator;
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
        [SerializeField] private Vector3[] path;
        [SerializeField] private int targetIndex;
        private Coroutine FollowPath_IE;

        [Header("Savings Data")]
        public CharacterProperties characterProperties;

        public static event Action OnCharacterLoaded;

        [Serializable]
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
            animator = GetComponent<Animator>();
            characterProperties.characterData = characterData;
        }

        protected virtual void Start()
        {
            Initialize();
            SetRandomFactors();
        }

        protected void SetRandomFactors()
        {
            speed = UnityEngine.Random.Range(3, 7);
            agility = UnityEngine.Random.Range(3, 7);
            resistance = UnityEngine.Random.Range(1, 5);
        }

        #region Following Path

        public virtual void FindPathToTarget(Vector3 target)
        {
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
        }

        protected virtual void OnPathFound(Vector3[] newPath, bool isPathSuccessful)
        {
            if (isPathSuccessful)
            {
                path = newPath;
                targetIndex = 0;

                StopMove();
                Move();
            }
        }

        protected virtual void Move()
        {
            FollowPath_IE = StartCoroutine(FollowPath());
        }

        protected virtual void StopMove()
        {
            if (FollowPath_IE != null)
            {
                FollowAnimation(false);
                StopCoroutine(FollowPath_IE);
            }
        }

        protected IEnumerator FollowPath()
        {
            if (!isSelected)
            {
                yield return new WaitForSeconds(2);
            }

            Vector3 currentWaypoint = path[0];
            targetIndex = 0;

            while (transform.position != path[path.Length - 1])
            {
                FollowAnimation(true);

                if (transform.position == currentWaypoint)
                {
                    targetIndex++;

                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentWaypoint - transform.position), agility * Time.deltaTime);
                yield return null;
            }

            FollowAnimation(false);
        }

        #endregion

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;

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

#endif

        #region Selecting

        public void Select()
        {
            characterHUD.SetCharacterSelected();
            isSelected = true;
        }

        public void Deselect()
        {
            characterHUD.SetCharacterDeselected();
            isSelected = false;
        }

        #endregion

        #region Animations

        private void FollowAnimation(bool isFollowing)
        {
            animator.SetBool("IsFollowing", isFollowing);
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

            if (loadedCharacter != null)
            {
                speed = loadedCharacter.speed;
                agility = loadedCharacter.agility;
                resistance = loadedCharacter.resistance;
                position = loadedCharacter.position;
                rotation = loadedCharacter.rotation;

                transform.localPosition = loadedCharacter.position;
                transform.localRotation = loadedCharacter.rotation;

                path = null;
                StopMove();
                OnCharacterLoaded?.Invoke();
            }

        }

        #endregion

    }
}


