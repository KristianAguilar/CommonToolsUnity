using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Data structure to set a dialog configuration with different types and amount of pages.
    /// </summary>
    [CreateAssetMenu(fileName = "DialogConfig", menuName = "Config/Dialog")]
    public class DialogConfig : ScriptableObject
    {
        /// <summary>
        /// Unique identifier to request this config.
        /// </summary>
        public string id;
        /// <summary>
        /// This config language.
        /// </summary>
        public SystemLanguage lang;
        /// <summary>
        /// All the pages attach to this dialog.
        /// </summary>
        public Page[] pages;
    }

    [System.Serializable]
    public class Page
    {
        /// <summary>
        /// Unique identifier for the dialog parent to request one specific page.
        /// </summary>
        public string pageId;
        /// <summary>
        /// Name of the entity or npc that is talking in this page.
        /// </summary>
        public string headerName;
        /// <summary>
        /// Position in the screen where the dialog panel will be show.
        /// </summary>
        public PagePosition position;
        /// <summary>
        /// Main text content.
        /// </summary>
        [TextArea] public string content;
    }

    [System.Serializable]
    public enum PagePosition
    {
        LeftBottom = 0,
        LeftMiddle = 1,
        LeftTop = 2,
        MiddleBottom = 3,
        MiddleMiddle = 4,
        MiddleTop = 5,
        RightBottom = 6,
        RightMiddle = 7,
        RightTop = 8,
    }
}