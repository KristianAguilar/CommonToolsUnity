using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    /// <summary>
    /// Service in charge of search and show dialogs in screen.
    /// </summary>
    public class DialogService : MonoBehaviour
    {   
        /// <summary>
        /// Global boolean to know if the player is currently in a dialog.
        /// </summary>
        public static bool inDialog { get; private set; }

        /// <summary>
        /// Action call when the player request a dialog with the current active npc.
        /// </summary>
        public static UnityAction<string> OnRequestStartDialog;


        // Start is called before the first frame update
        void Start()
        {
            OnRequestStartDialog += RequestNewDialog;
        }

        /// <summary>
        /// Manage dialog request and alert other script that the dialog starts.
        /// </summary>
        /// <param name="dialogId">unique id to find the dialog class</param>
        private void RequestNewDialog(string dialogId)
        {
            inDialog = true;
        }

        private void Update()
        {
            // place holder stop dialog
            if (inDialog && Input.GetKey(KeyCode.H))
            {
                inDialog = false;
            }
        }
    }
}