using UnityEngine;

namespace CommonTools
{
    /// <summary>
    /// Dont destroy this game object and all it's scripts on load a new scene
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}