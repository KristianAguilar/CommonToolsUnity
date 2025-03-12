using System.Collections.Generic;
using System.Linq;
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
        /// Prefab to instantiate a canvas that manages pages.
        /// </summary>
        [SerializeField] private GameObject pageUIPrefab;
        /// <summary>
        /// Current language dialogs
        /// </summary>
        [SerializeField] private List<DialogConfig> Dialogs;

        /// <summary>
        /// Global boolean to know if the player is currently in a dialog.
        /// </summary>
        public static bool inDialog { get; private set; }
        /// <summary>
        /// Action call when the player request a dialog with the current active npc.
        /// </summary>
        public static UnityAction<string> OnRequestStartDialog;

        /// <summary>
        /// Current service name use for logs.
        /// </summary>
        private const string SERVICE_NAME = "<color=magenta>Dialog Service: </color>";
        /// <summary>
        /// Current dialog in screen.
        /// </summary>
        private DialogConfig currentDialog;
        /// <summary>
        /// Instance of the class that manages the pages.
        /// </summary>
        private PageUI pageUI;
        /// <summary>
        /// Alternative access to the current dialog pages.
        /// </summary>
        private List<Page> currentPages;
        /// <summary>
        /// The next page to show after the current. null means dialog finish.
        /// </summary>
        private Page nextPage;

        // Start is called before the first frame update
        void Start()
        {
            OnRequestStartDialog += RequestNewDialog;
            GameObject pageInstance = Instantiate(pageUIPrefab, transform);
            pageUI = pageInstance.GetComponent<PageUI>();
            pageUI.OnPageFinish += SetupNextPage;
            currentPages = new List<Page>();
        }

        /// <summary>
        /// Manage dialog request and alert other script that the dialog starts.
        /// </summary>
        /// <param name="dialogId">unique id to find the dialog class</param>
        private void RequestNewDialog(string dialogId)
        {
            Debug.Log($"{SERVICE_NAME}Starting to search and show dialog: {dialogId}");
            
            DialogConfig dialog = Dialogs.Find(d => d.id == dialogId);
            if (dialog == null)
            {
                Debug.LogError($"{SERVICE_NAME}dialog {dialogId} not found.");
                FinishDialog();
                return;
            }
             
            inDialog = true;
            currentDialog = dialog;
            currentPages = currentDialog.pages.ToList();

            // hard setup of the next page to show
            nextPage = currentDialog.pages[0];
            RequestPage();
        }

        /// <summary>
        /// Setup the next page to request using the current page and page id in case of options.
        /// </summary>
        /// <param name="pageId">next page to reques, string empty to follow index order.</param>
        private void SetupNextPage(string pageId = null)
        {
            if (string.IsNullOrEmpty(pageId))
            {
                // use the current value of 'nextPage' (just show) to update to the actual next.
                int index = currentPages.IndexOf(nextPage);

                if (index + 1 < currentPages.Count)
                    nextPage = currentPages[index + 1];
                else
                    nextPage = null;
            }
            else
            {
                // user request to jump to an specific page, used in options page. 
                Page page = currentPages.Find(p => p.pageId == pageId);
                if (page == null)
                {
                    Debug.LogError($"{SERVICE_NAME}request page {page} not found.");
                    nextPage = null;
                }
            }
            

            RequestPage();
        }

        /// <summary>
        /// Request the UI to show the page saved in the nextPage variable.
        /// </summary>
        private void RequestPage()
        {
            if (nextPage != null)
            {
                pageUI.SetupPage(nextPage);
            }
            else
            {
                FinishDialog();
            }
        }

        private void FinishDialog()
        {
            inDialog = false;
            currentDialog = null;
            nextPage = null;
            currentPages = new List<Page>();
        }

    }
}