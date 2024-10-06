using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NotificationService
{
    /// <summary>
    /// Notication with fields to show a simple message with one button
    /// </summary>
    public class SimpleNotification : MonoBehaviour
    {
        /// <summary>Message to show in the center</summary>
        [SerializeField] private TMP_Text _message;
        /// <summary> Button txt</summary>
        [SerializeField] private TMP_Text _buttonTxt;
        /// <summary> Image to attach a sprite icon</summary>
        [SerializeField] private Image _iconImg;
        /// <summary> Back overlay with alpha behind the notification</summary>
        [SerializeField] private GameObject _overlay;

        /// <summary> Action to execute the button.</summary>
        private UnityAction _action;

        /// <summary>Use the left button options to setup the ui prefab</summary>
        /// <param name="config"></param>
        public void Setup(NotificationConfig config)
        {
            _message.text = config.message;
            _buttonTxt.text = config.leftButtonText;
            _iconImg.sprite = config.sprite;
            _action = config.onLeftButtonPress;
            _overlay.SetActive(!config.hideOverlay);
        }

        /// <summary>Action on click the button</summary>
        public void InvokeAction()
        {
            _action?.Invoke();
        }
    }
}