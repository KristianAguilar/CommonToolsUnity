using System.IO;
using UnityEngine;

namespace SaveFiles
{
    /// <summary>
    /// Service to allow save and load generic template data class
    /// </summary>
    public class SaveFilesService
    {
        private static string DEFAULT_FOLDER = "AppFiles";
        private static string SERVICE_NAME = "<color=cyan>SaveFilesService</color>";

        /// <summary>Try to load a json data file if exist and load to the given class</summary>
        /// <typeparam name="T">Data class type to save</typeparam>
        /// <param name="fileName">File name with extension, ex. data.json</param>
        /// <param name="data">Data class instance</param>
        /// <param name="customFolder">can be null. Use to save the file in a custom folder name</param>
        /// <returns>True file save with success, false otherwise.</returns>
        public static bool SaveDataFile<T>(string fileName, T data, string customFolder = null)
        {
            fileName = FixedFilePath(fileName, customFolder);
            CreateFilesFolder(customFolder);

            string jsonTxt = JsonUtility.ToJson(data, true);
            File.WriteAllText(fileName, jsonTxt);
            if (File.Exists(fileName))
            {
                Debug.Log($"{SERVICE_NAME}: File saved with success! path: {fileName}.");
                return true;
            }
            else
            {
                Debug.LogError($"{SERVICE_NAME}: Error on save the file at path: {fileName}.");
                return false;
            }
        }


        /// <summary>Try to load a json data file if exist and load to the given class</summary>
        /// <typeparam name="T">Data class type to return</typeparam>
        /// <param name="fileName">File name with extension, ex. data.json</param>
        /// <param name="data">reference class for return the loaded data</param>
        /// <param name="customFolder">can be null. Use to load and save the file in a custom folder name</param>
        /// <returns>False if file not found or other problem, true otherwise.</returns>
        public static bool TryToLoadDataFile<T>(string fileName, out T data, string customFolder = null)
        {
            fileName = FixedFilePath(fileName, customFolder);

            if (!File.Exists(fileName))
            {
                data = default;
                Debug.Log($"{SERVICE_NAME}: file name not found: {fileName}");
                return false;
            }
            // File exist, load and return data
            string jsonTxt = File.ReadAllText(fileName);
            data = JsonUtility.FromJson<T>(jsonTxt);
            Debug.Log($"{SERVICE_NAME}: file name loaded with success: {fileName}");
            return true;
        }

        /// <summary>Return a fixed version of the path string, with folder name.</summary>
        /// <param name="filePath">file name with extension</param>
        /// <param name="customFolder">custom folder, leave empty to use default private folder</param>
        private static string FixedFilePath(string filePath, string customFolder = null)
        {
            if (string.IsNullOrEmpty(customFolder))
                filePath = Path.Combine(Application.persistentDataPath, DEFAULT_FOLDER, filePath);
            else
                filePath = Path.Combine(Application.persistentDataPath, customFolder, filePath);

            return filePath;
        }

        /// <summary>Create the files folder directory if doesn't exist.</summary>
        /// <param name="customFolder">empty to use the default path</param>
        private static void CreateFilesFolder(string customFolder = null)
        {
            string folder;
            if (string.IsNullOrEmpty(customFolder))
                folder = Path.Combine(Application.persistentDataPath, DEFAULT_FOLDER);
            else
                folder = Path.Combine(Application.persistentDataPath, customFolder);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        /// <summary>Return an array of file paths in the default or custom directory</summary>
        /// <param name="customFolder">leave null to use default</param>
        public static string[] GetFilePaths(string customFolder = null)
        {
            string folder = string.Empty;
            if (string.IsNullOrEmpty(customFolder))
                folder = Path.Combine(Application.persistentDataPath, DEFAULT_FOLDER);
            else
                folder = Path.Combine(Application.persistentDataPath, customFolder);

            if (Directory.Exists(folder))
                return Directory.GetFiles(folder);
            else
                return null;
        }
    }
}
