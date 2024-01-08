using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestCharactersMovement.SaveLoadSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager instance { get; private set; }

        [Header("Saving options")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption = false;

        private GameData gameData;
        private FileDataHandler fileDataHandler;
        private List<ISaveLoadData> saveLoadDataObjects;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            NewGame();
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            saveLoadDataObjects = FindAllSaveLoadDataObjecs();
        }

        public void NewGame() => gameData = new GameData();

        public void SaveGame()
        {
            foreach (ISaveLoadData saveLoadDataObject in saveLoadDataObjects)
            {
                saveLoadDataObject.Save(gameData);
            }

            fileDataHandler.Save(gameData);
        }

        public void LoadGame()
        {
            gameData = fileDataHandler.Load();

            if (gameData == null) { NewGame(); }

            foreach (ISaveLoadData saveLoadDataObject in saveLoadDataObjects)
            {
                saveLoadDataObject.Load(gameData);
            }
        }

        private List<ISaveLoadData> FindAllSaveLoadDataObjecs()
        {
            IEnumerable<ISaveLoadData> saveLoadDataObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveLoadData>();

            return new List<ISaveLoadData>(saveLoadDataObjects);
        }


    }

   

}


