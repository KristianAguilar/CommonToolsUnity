using UnityEngine;


namespace ThirdPersonDollTemplate
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float rotationSpeed = 5f;
        
        private CharacterController controller;
        
        private Vector3 input;


        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = 0;
            input.z = Input.GetAxis("Vertical");

            Move();
        }

        private void Move()
        {
            if (!controller.isGrounded)
            {
                Vector3 direction = new Vector3(0f, -9.81f * Time.deltaTime, 0f);
                controller.Move(direction);
            }
            else if (controller.isGrounded && input.magnitude != 0)
            {
                Vector3 direction = input;//cameraTransform.TransformDirection(input);

                direction.y = 0;
                Debug.DrawLine(transform.position, transform.position + direction * 3f, Color.magenta, 2f);

                Quaternion targetDirection = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetDirection, Time.deltaTime * rotationSpeed);

                controller.Move(direction * Time.deltaTime * speed);
            }
        }

    }
}
