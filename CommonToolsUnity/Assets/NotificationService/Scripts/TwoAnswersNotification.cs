using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NotificationService
{
    public class TwoAnswersNotification : MonoBehaviour
    {
        /// <summary>Message to show in the center</summary>
        [SerializeField] private TMP_Text _message;
        /// <summary>Left Button txt</summary>
        [SerializeField] private TMP_Text _leftButtonTxt;
        /// <summary>Right Button txt</summary>
        [SerializeField] private TMP_Text _rightButtonTxt;
        /// <summary> Image to attach a sprite icon</summary>
        [SerializeField] private Image _iconImg;
        /// <summary> Back overlay with alpha behind the notification</summary>
        [SerializeField] private GameObject _overlay;

        /// <summary> Action to execute the button.</summary>
        private UnityAction _leftAction;
        private UnityAction _rightAction;

        /// <summary>Use the left button options to setup the ui prefab</summary>
        /// <param name="config"></param>
        public void Setup(NotificationConfig config)
        {
            _message.text = config.message;
            _iconImg.sprite = config.sprite;
            _overlay.SetActive(!config.hideOverlay);

            _leftButtonTxt.text = config.leftButtonText;
            _rightButtonTxt.text = config.rightButtonText;

            _leftAction = config.onLeftButtonPress;
            _rightAction = config.onRightButtonPress;
        }

        /// <summary>Action on click the left button</summary>
        public void LeftAction()
        {
            _leftAction?.Invoke();
        }

        /// <summary>Action on click the right button</summary>
        public void RightAction()
        {
            _rightAction?.Invoke();
        }
    }
}
