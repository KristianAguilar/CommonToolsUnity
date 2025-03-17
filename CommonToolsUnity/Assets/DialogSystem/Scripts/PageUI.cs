using System;
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

        private const string HTML_ALPHA = "<color=#00000000>";

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
            SetupPagePosition(page.position);
        }

        /// <summary>
        /// Routine to typing the main content slowly letter after letter
        /// </summary>
        private IEnumerator TypingContent()
        {
            if (currentPage == null)
                yield break;

            finishTypingUI.SetActive(false);
            int alphaIndex = 1;

            content.text = ""; 
            foreach (char letter in currentPage.content)
            {
                if (currentPage == null)
                    yield break;

                content.text = currentPage.content;
                string textEdited = content.text.Insert(alphaIndex, HTML_ALPHA);
                content.text = textEdited;
                alphaIndex++;
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
                    if (!finishTypingUI.activeSelf)
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

        /// <summary>
        /// Change the position of the main panel depending on the page config.
        /// </summary>
        /// <param name="position">position in the screen where the dialog will be show</param>
        private void SetupPagePosition(PagePosition position)
        {
            switch (position)
            {
                case PagePosition.LeftBottom:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(-250, -150, 0f);
                    break;
                case PagePosition.MiddleBottom:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(0f, -150, 0f);
                    break;
                case PagePosition.RightBottom:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(250, -150, 0f);
                    break;

                case PagePosition.LeftMiddle:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(-250, 0f, 0f);
                    break;
                case PagePosition.MiddleMiddle:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                    break;
                case PagePosition.RightMiddle:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(250, 0f, 0f);
                    break;

                case PagePosition.LeftTop:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(-250, 150f, 0f);
                    break;
                case PagePosition.MiddleTop:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
                    break;
                case PagePosition.RightTop:
                    mainPanel.GetComponent<RectTransform>().localPosition = new Vector3(250, 150f, 0f);
                    break;

            }
        }
    }
}
