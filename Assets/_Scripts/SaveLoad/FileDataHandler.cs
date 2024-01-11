using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TestCharactersMovement.SaveLoadSystem
{
    public class FileDataHandler
    {

        private string dirPath = string.Empty;
        private string fileName = string.Empty;

        private bool useEncryption = false;

        private readonly string encryptionCodeWord = "Secret";

        public FileDataHandler(string dirPath, string fileName, bool useEncryption)
        {
            this.dirPath = dirPath;
            this.fileName = fileName;
            this.useEncryption = useEncryption;
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(dirPath, fileName);
            GameData loadedData = null;

            Debug.Log(fullPath);

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = string.Empty;

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecryptData(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

                }
                catch (Exception e)
                {
                    Debug.LogError("Error during loading data from file: " + fullPath + "\n" + e);
                }
            }

            return loadedData;

        }

        public async void Save(GameData data)
        {
            string fullPath = Path.Combine(dirPath, fileName);

            try
            {
               await Task.Delay(1000);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToSave = JsonUtility.ToJson(data, true);

                if (useEncryption)
                {
                    dataToSave = EncryptDecryptData(dataToSave);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToSave);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogError("Error during saving data to file: " + fullPath + "\n" + e);
            }
        }

        private string EncryptDecryptData(string data)
        {
            string modifiedData = string.Empty;

            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }

            return modifiedData;
        }


    }
}


