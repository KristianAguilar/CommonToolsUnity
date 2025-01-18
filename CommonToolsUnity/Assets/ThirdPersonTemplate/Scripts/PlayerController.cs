using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonSenital
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _walkSpeed = 5f;
        [SerializeField] private float _turningSpeed = 2f;
        [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _sprintSpeed = 10f;
        [SerializeField] private float _sprintTransitSpeed = 5f;

        private CharacterController _controller;

        private Vector2 _input;

        private float _verticalVelocity;
        private float _speed;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            _input.x = Input.GetAxis("Horizontal");
            _input.y = Input.GetAxis("Vertical");

            Move();
            Turn();
        }

        private void Move()
        {
            Vector3 movement = new Vector3(_input.x, 0f, _input.y);
            movement = _camera.TransformDirection(movement);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _speed = Mathf.Lerp(_speed, _sprintSpeed, _sprintTransitSpeed * Time.deltaTime);
            }
            else
            {
                _speed = Mathf.Lerp(_speed, _walkSpeed, _sprintTransitSpeed * Time.deltaTime);
            }

            movement *= _speed;

            movement.y = VerticalForceCalculation();

            _controller.Move(movement * Time.deltaTime);
        }

        private void Turn()
        {
            if (_input.magnitude != 0 && _controller.velocity.magnitude != 0)
            {
                Vector3 currentLookDirection = _controller.velocity.normalized;
                currentLookDirection.y = 0;
                currentLookDirection.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(currentLookDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _turningSpeed);
            }
        }

        private float VerticalForceCalculation()
        {
            if (_controller.isGrounded)
            {
                _verticalVelocity = -1;

                if (Input.GetButtonDown("Jump"))
                {
                    _verticalVelocity = Mathf.Sqrt(_gravity * _jumpHeight * 2);
                }
            }
            else
            {
                _verticalVelocity -= _gravity * Time.deltaTime;
            }
            return _verticalVelocity;
        }
    }
}
