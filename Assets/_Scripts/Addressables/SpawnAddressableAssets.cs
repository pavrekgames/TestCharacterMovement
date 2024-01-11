using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TestCharactersMovement.Addressables
{
    public class SpawnAddressableAssets : MonoBehaviour
    {

        [SerializeField] private AssetReferenceGameObject environment;

        public static event Action OnObjectsSpawned;

        private void Start()
        {
            SpawnObjects();
        }

        private void SpawnObjects()
        {
            environment.InstantiateAsync().Completed += OnAddressableLoaded;
        }

        private void OnAddressableLoaded(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(handle.Result);
                OnObjectsSpawned?.Invoke();
            }
            else
            {
                Debug.LogError("Loading Asset Failed");
            }
        }

    }
}


