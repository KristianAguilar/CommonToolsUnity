using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotificationService
{
    /// <summary>
    /// Script to test arise notifications
    /// </summary>
    public class DemoNotification : MonoBehaviour
    {
        private void Update()
        {
            if (NotificationService.Instance.notificationActive)
                return;

            if (Input.GetKey(KeyCode.S))
            {
                Simple();
            }

            if (Input.GetKey(KeyCode.T))
            {
                TwoAnswers();
            }
        }

        private void Simple()
        {
            NotificationService.Instance.AriseSimpleNotification(new NotificationConfig
            {
                message = "Prueba notificación simple",
                leftButtonText = "Continuar",
                type = NotificationType.Warning,
                // onLeftButtonPress = here add custom actions, also will close the notification.
            });
        }

        private void TwoAnswers()
        {
            NotificationService.Instance.AriseTwoAnswersNotification(new NotificationConfig
            {
                message = "Prueba notificación con dos opciones",
                leftButtonText = "Aceptar",
                rightButtonText = "Cancelar",
                onLeftButtonPress = () => Debug.Log("Left button action"),
                onRightButtonPress = () => Debug.Log("Right button action"),
                hideOverlay = true, //--> to hide the background overlay
                type = NotificationType.Informative,
            });
        }
    }
}