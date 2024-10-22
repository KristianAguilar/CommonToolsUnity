using System.IO;
using UnityEngine;

namespace ManageFileService
{
    /// <summary>
    /// Service to allow manage the CRUD process for files in the persistent data memory.
    /// </summary>
    public class ManageFilesService : MonoBehaviour
    {
        /// <summary>
        /// Service singleton to have access to it, call the service using ManageFilesService.Instance...
        /// </summary>
        public static ManageFilesService Instance { get; private set; }

        /// <summary>
        /// Service string name to log
        /// </summary>
        private static string SERVICE_NAME = "<color=yellow>ManageFileService</color>";

        private string _defaultFolderPath;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
        }

        private void Start()
        {
            _defaultFolderPath = Path.Combine(Application.persistentDataPath, "Default");
            if (!Directory.Exists(_defaultFolderPath))
            {
                Directory.CreateDirectory(_defaultFolderPath);
            }
        }

        /// <summary>
        /// Save the serializa data in the default folder using the given file name.
        /// </summary>
        /// <param name="fileName">file name, includes the extension</param>
        /// <param name="jsonData">json serialize data to save</param>
        /// <returns>Path where the file was saved, empty on error</returns>
        public string SaveFile(string fileName, string jsonData)
        {
            if (string.IsNullOrEmpty(fileName) || !fileName.Contains("."))
            {
                Debug.LogError($"{SERVICE_NAME}: Not valid file name to be saved {fileName}.");
                return string.Empty;
            }

            string filePath = Path.Combine(_defaultFolderPath, fileName);

            File.WriteAllText(filePath, jsonData);
            if (File.Exists(filePath))
            {
                Debug.Log($"{SERVICE_NAME}: File saved with success! path: {filePath}.");
                return filePath;
            }
            else
            {
                Debug.LogError($"{SERVICE_NAME}: Can't save the file path: {filePath}.");
                return string.Empty;
            }
        }

        /// <summary>
        /// Save the serializa data in a custom folder using the given file name.
        /// </summary>
        /// <param name="fileName">file name, includes the extension</param>
        /// <param name="jsonData">json serialize data to save</param>
        /// <param name="folderName">folder name</param>
        /// <returns>Path where the file was saved, empty on error</returns>
        public string SaveFile(string fileName, string jsonData, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || !fileName.Contains("."))
            {
                Debug.LogError($"{SERVICE_NAME}: Not valid file name to be saved {fileName}.");
                return string.Empty;
            }

            folderName = Path.Combine(Application.persistentDataPath, folderName);
            
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string filePath = Path.Combine(folderName, fileName);

            File.WriteAllText(filePath, jsonData);
            if (File.Exists(filePath))
            {
                Debug.Log($"{SERVICE_NAME}: File saved with success! path: {filePath}.");
                return filePath;
            }
            else
            {
                Debug.LogError($"{SERVICE_NAME}: Can't save the file path: {filePath}.");
                return string.Empty;
            }
        }

        /// <summary>
        /// Transform serializable class to json, and save it in a default folder.
        /// </summary>
        /// <typeparam name="T">Serialazible class</typeparam>
        /// <param name="fileName">file name in memory</param>
        /// <param name="folderName">folder name for the file</param>
        /// <returns>Path where the file was saved, empty on error</returns>
        public string SaveClassToFile<T>(string fileName, T serializeData)
        {
            string jsonText = JsonUtility.ToJson(serializeData, true);
            return SaveFile(fileName, jsonText);
        }

        /// <summary>
        /// Transform serializable class to json, and save it.
        /// </summary>
        /// <typeparam name="T">Serialazible class</typeparam>
        /// <param name="fileName">file name in memory</param>
        /// <param name="folderName">folder name for the file</param>
        /// <returns>Path where the file was saved, empty on error</returns>
        public string SaveClassToFile<T>(string fileName, T serializeData, string folderName)
        {
            string jsonText = JsonUtility.ToJson(serializeData, true);
            return SaveFile(fileName, jsonText, folderName);
        }

        /// <summary>Load file by7 path and return the serializable class.</summary>
        /// <typeparam name="T">serialize class type</typeparam>
        /// <param name="filePath">path where the file should be</param>
        /// <returns></returns>
        public T LoadFile<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonText = File.ReadAllText(filePath);
                Debug.Log($"{SERVICE_NAME}: File loaded with success! path: {filePath}.");
                return JsonUtility.FromJson<T>(jsonText);
            }
            else
            {
                Debug.LogError($"{SERVICE_NAME}: Can't load the file path: {filePath}.");
                return default;
            }
        }
    }
}
