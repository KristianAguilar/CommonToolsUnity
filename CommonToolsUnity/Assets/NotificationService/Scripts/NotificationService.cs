using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Services
{
    /// <summary>
    /// Service that allow to show notifications of different types in screen,
    /// use a background overlay and autoclose on click any option.
    /// </summary>
    public class NotificationService : MonoBehaviour
    {
        /// <summary>
        /// Services singleton to have access to it, call the services using NotificationService.Instance...
        /// </summary>
        public static NotificationService Instance { get; private set; }

        /// <summary>
        /// Prefab with the ui items to show a simple notification with one message and button.
        /// </summary>
        [SerializeField] private GameObject _simpleNotificationPrefab;

        [SerializeField] private GameObject _twoAnswerNotificationPrefab;

        /// <summary>
        /// List of tuples of sprites notification types 
        /// </summary>
        [SerializeField] private List<SpriteType> spriteTypes;

        /// <summary>
        /// Current notification instance != null if it's in the screen
        /// </summary>
        private GameObject _notificationInstance;

        /// <summary>
        /// True if there is a notification in the screen, false otherwise
        /// </summary>
        public bool notificationActive => _notificationInstance != null;


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

        /// <summary> Arise a notification in the screen with one message and button option</summary>
        /// <param name="config">config with the necessary data to load 1 button. Can have empty fields</param>
        public void AriseSimpleNotification(OneOptionConfig config)
        {
            if (_notificationInstance != null)
            {
                Debug.LogWarning("There is already a notification in the screen");
                return;
            }

            // create instance notification
            _notificationInstance = Instantiate(_simpleNotificationPrefab);
            SimpleNotification simpleNotification = _notificationInstance.GetComponent<SimpleNotification>();
            
            // fix notification fields
            config.onButtonPress += CloseNotification;

            if (config.iconSprite == null)
                config.iconSprite = GetNotificationSprite(config.type);
            
            // setup the ui
            simpleNotification.Setup(config);
        }

        /// <summary> Arise a notification in screen with 2 buttons</summary>
        /// <param name="config">config with the necessary data to load 2 button info</param>
        public void AriseTwoAnswersNotification(TwoOptionsConfig config)
        {
            if (_notificationInstance != null)
            {
                Debug.LogWarning("There is already a notification in the screen");
                return;
            }
            // create instance notification
            _notificationInstance = Instantiate(_twoAnswerNotificationPrefab);
            TwoAnswersNotification twoAnswerNotification = _notificationInstance.GetComponent<TwoAnswersNotification>();

            // fix notification fields
            config.onLeftButtonPress += CloseNotification;
            config.onRightButtonPress += CloseNotification;
            config.sprite = GetNotificationSprite(config.type);

            // setup the ui
            twoAnswerNotification.Setup(config);
        }

        /// <summary> Close the notification by destroy the instance in screen. </summary>
        private void CloseNotification()
        {
            Destroy(_notificationInstance);
            _notificationInstance = null;
        }

        /// <summary> Find a sprite for the requested notification type</summary>
        /// <param name="type"></param>
        /// <returns>Sprite attach to type</returns>
        private Sprite GetNotificationSprite(NotificationType type)
        {
            return spriteTypes.Find( (sp) => sp.type == type ).sprite;
        }

    }

    /// <summary>
    /// Configuration struct with the data to load a notification
    /// </summary>
    public struct TwoOptionsConfig
    {
        public string message;
        public string leftButtonText;
        public UnityAction onLeftButtonPress;
        public string rightButtonText;
        public UnityAction onRightButtonPress;
        public NotificationType type;
        public Sprite sprite;
        public bool hideOverlay;
    }

    /// <summary>
    /// Config to create a simple notification
    /// </summary>
    public struct OneOptionConfig
    {
        /// <summary>
        /// Main message show to the user.
        /// </summary>
        public string message;
        /// <summary>
        /// Text in the button text.
        /// </summary>
        public string buttonText;
        /// <summary>
        /// Action on press the button. autoclose is already included.
        /// </summary>
        public UnityAction onButtonPress;
        /// <summary>
        /// Type info, warning, or error.
        /// </summary>
        public NotificationType type;
        /// <summary>
        /// Can use a default sprite in the notification icon. Null use default sprites.
        /// </summary>
        public Sprite iconSprite;
        /// <summary>
        /// True for hide the background
        /// </summary>
        public bool hideOverlay;
    }

    [System.Serializable]
    public struct SpriteType
    {
        public NotificationType type;
        public Sprite sprite;
    }

    public enum NotificationType
    {
        Informative = 0,
        Warning = 1,
        Error = 2,
    }
}