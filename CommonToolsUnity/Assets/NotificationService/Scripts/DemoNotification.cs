using UnityEngine;

namespace Services
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
            NotificationService.Instance.AriseSimpleNotification(new OneOptionConfig
            {
                message = "Prueba notificación simple",
                buttonText = "Continuar",
                type = NotificationType.Warning,
                onButtonPress = ButtonPressAlert,
                //hideOverlay = true // default false => show bg overlay
            });
        }

        private void ButtonPressAlert()
        {
            Debug.Log("Simple notification close");
        }

        private void TwoAnswers()
        {
            NotificationService.Instance.AriseTwoAnswersNotification(new TwoOptionsConfig
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