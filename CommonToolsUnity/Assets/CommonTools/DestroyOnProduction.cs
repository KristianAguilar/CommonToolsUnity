using UnityEngine;

namespace CommonTools
{
    /// <summary>
    /// Destroy this gameobject and all it's script on production environment.
    /// </summary>
    public class DestroyOnProduction : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (!Debug.isDebugBuild)
                Destroy(this.gameObject);
        }
    }
}