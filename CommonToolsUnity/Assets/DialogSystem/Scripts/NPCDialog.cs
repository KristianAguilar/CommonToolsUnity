using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Trigger a dialog screen associated with this NPC enity. Must be attach to
    /// the collider that will detect the player trigger.
    /// </summary>
    public class NPCDialog : MonoBehaviour
    {
        /// <summary>
        /// Reference to the npc transform to get the propper position.
        /// </summary>
        [SerializeField] private Transform NPCRef;
        /// <summary>
        /// Icon that will be on the top of the NPC.
        /// </summary>
        [SerializeField] private GameObject dialogIconPrefab;
        /// <summary>
        /// Unique id to search the propper dialog.
        /// </summary>
        [SerializeField] private string dialogId;

        /// <summary>
        /// Current npc with the dialog icon arise. Null by default.
        /// </summary>
        private static NPCDialog currenDialogArise;
        /// <summary>
        /// Instance of the dialog icon prefab, to avoid destroy and instantiate.
        /// </summary>
        private GameObject dialogIconInstance;


        private void OnTriggerStay(Collider other)
        {
            if (!DialogService.inDialog && other.gameObject.CompareTag("Player"))
            {
                PlayerClose(other.gameObject.transform.position);
            }
        }

        /// <summary>
        /// Try to arise the dialog icon to start this npc dialog.
        /// </summary>
        private void PlayerClose(Vector3 playerPosition)
        {
            // 1.First case no other npc arise the dialog icon
            if (currenDialogArise == null)
            {
                currenDialogArise = this;
                AriseDialogIcon();
            }
            // 2.Other npc has the icon taken
            else if (currenDialogArise != null && currenDialogArise != this)
            {
                // Compare distance between NPCs
                float distanceToMe = Vector3.Distance(playerPosition, NPCRef.position);
                float distanceToOther = Vector3.Distance(playerPosition, currenDialogArise.transform.position);

                if (distanceToMe < distanceToOther)
                {
                    currenDialogArise.HideDialogIcon();
                    currenDialogArise = this;
                    AriseDialogIcon();
                }
            }
            // 3.Nothing because this is assing
        }

        /// <summary>
        /// Arise a dialog icon over the NPC
        /// </summary>
        private void AriseDialogIcon()
        {
            if (dialogIconInstance == null)
                dialogIconInstance = Instantiate(dialogIconPrefab, NPCRef.transform);
            else
                dialogIconInstance.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!DialogService.inDialog && other.gameObject.CompareTag("Player"))
            {
                PlayerLeaves();
            }
        }

        /// <summary>
        /// Reac to the player away from the collider.
        /// </summary>
        private void PlayerLeaves()
        {
            if (currenDialogArise == this)
            {
                HideDialogIcon();
                currenDialogArise = null;
            }
        }

        /// <summary>
        /// Hide this npc dialog icon if exist.
        /// </summary>
        public void HideDialogIcon() 
        {
            if (dialogIconInstance != null)
                dialogIconInstance.SetActive(false);
        }

        private void Update()
        {
            // Manage try to start a new dialog
            if (!DialogService.inDialog && currenDialogArise == this)
            {
                // only here read player input
                if (Input.GetAxis("Fire1") != 0)
                {
                    DialogService.OnRequestStartDialog?.Invoke(dialogId);
                }
            }
        }

        private void OnDisable()
        {
            if (dialogIconInstance != null)
                Destroy(dialogIconInstance);
        }
    }
}
