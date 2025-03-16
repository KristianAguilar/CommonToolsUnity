using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    /// <summary>
    /// Frontend class in charge of the setup a page and return tothe DialogService on finish showing. 
    /// Also manage the user input to skip the typing.
    /// </summary>
    public class PageUI : MonoBehaviour
    {
        /// <summary>
        /// Dialog global panel that turn on and off on starts dialogs.
        /// </summary>
        [SerializeField] private GameObject mainPanel;
        /// <summary>
        /// NPC, Player or entity that is talking in this page.
        /// </summary>
        [SerializeField] private TMP_Text header;
        /// <summary>
        /// Main page content.
        /// </summary>
        [SerializeField] private TMP_Text content;
        /// <summary>
        /// UI Element to tell the player the current content was wrote.
        /// </summary>
        [SerializeField] private GameObject finishTypingUI;
        /// <summary>
        /// Speed of typing a word after other.
        /// </summary>
        [SerializeField, Range(0.001f, 1f)] private float typingCooldown = 0.04f;
        /// <summary>
        /// Seconds to allow the user skip the current page
        /// </summary>
        [SerializeField] private float skipDialogTime = 0.5f;

        /// <summary>
        /// Current Page in the screen.
        /// </summary>
        private Page currentPage;
        /// <summary>
        /// True if allow the user to skip the current dialog. Used to avoid multiple fire 1 input.
        /// </summary>
        private bool allowSkipTyping;

        /// <summary>
        /// Action on the current page typing finish, you can send the next page id if its a option page.
        /// Send a empty string as default value.
        /// </summary>
        public UnityAction<string> OnPageFinish;

        void OnEnable()
        {
            mainPanel.SetActive(false);
            currentPage = null;
            finishTypingUI.SetActive(false);
            allowSkipTyping = false;
        }

        /// <summary>
        /// Setup a new page to show in the screen.
        /// </summary>
        /// <param name="page"></param>
        public void SetupPage(Page page)
        {
            currentPage = page;
            
            if (!mainPanel.activeSelf) // first page only
            {
                mainPanel.SetActive(true);
                // TODO: here pop up dialog screen animation
            }

            header.text = currentPage.headerName;
            StartCoroutine(AllowSkipTypingCooldown());
            allowSkipTyping = false;
            StartCoroutine(TypingContent());
        }

        /// <summary>
        /// Routine to typing the main content slowly letter after letter
        /// </summary>
        private IEnumerator TypingContent()
        {
            if (currentPage == null)
                yield break;

            content.text = "";
            finishTypingUI.SetActive(false);
            foreach (char letter in currentPage.content)
            {
                if (currentPage == null)
                    yield break;

                content.text += letter;
                yield return new WaitForSeconds(typingCooldown);
            }

            finishTypingUI.SetActive(true);
        }

        private void Update()
        {
            if (currentPage != null)
            {
                if ((Input.GetAxis("Fire1") != 0 || Input.GetKeyUp(KeyCode.E) ) && allowSkipTyping)
                {
                    if (content.text != currentPage.content)
                    {
                        SkipTyping();
                        allowSkipTyping = false;
                        StartCoroutine(AllowSkipTypingCooldown());
                    }
                    else
                        PageFinish();
                }
            }
        }

        /// <summary>
        /// Skip the slow text typing and show all the page.
        /// </summary>
        private void SkipTyping()
        {
            StopAllCoroutines();
            content.text = currentPage.content;
            finishTypingUI.SetActive(true);
        }

        /// <summary>
        /// Page was show, and player request the next action.
        /// </summary>
        private void PageFinish()
        {
            mainPanel.SetActive(false);
            currentPage = null;
            finishTypingUI.SetActive(false);
            OnPageFinish?.Invoke(string.Empty);
        }

        private IEnumerator AllowSkipTypingCooldown()
        {
            yield return new WaitForSeconds(skipDialogTime);
            allowSkipTyping = true;
        }
    }
}
