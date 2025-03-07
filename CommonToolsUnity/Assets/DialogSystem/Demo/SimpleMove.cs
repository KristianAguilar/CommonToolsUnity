using DialogSystem;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] private float speed = 8f;

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (DialogService.inDialog)
            return;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (input.magnitude != 0)
        {
            controller.Move(input * speed *Time.deltaTime);
        }
    }
}
