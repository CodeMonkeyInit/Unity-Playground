using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityTemplateProjects
{
    public enum UserActionType
    {
        HorizontalMovement,
        VerticalMovement,
        Shoot
    }

    public struct Camera
    {
        public float x;
        public float y;
    }

    public class CameraController
    {
        public float sensitivity = 300f;

        public Camera GetCameraInput()
        {
            var xAxis = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
            var yAxis = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

            return new Camera
            {
                x = xAxis,
                y = yAxis
            };
        }
    }

    public class InputController
    {

        private Dictionary<int, UserActionType> mouseButtonBindings = new Dictionary<int, UserActionType>()
        {
            {0, UserActionType.Shoot}
        };

        public List<UserAction> GetUserInputs()
        {
            var userActions = mouseButtonBindings.Keys.Where(Input.GetMouseButton)
                .Select(mouseButtonKey => new UserAction {type = mouseButtonBindings[mouseButtonKey]}).ToList();
            

            return userActions;
        }
    }

    public struct UserAction
    {
        public float value;

        public UserActionType type;
    }


    public class UserMovementsController : MonoBehaviour
    {

        public CharacterController characterController;
        public Transform groundCheck;
        public float groundDistance = .4f;
        public LayerMask groundMask;
        
        public float speed = 10f;

        private InputController _inputController;
        
        private Vector3 _velocity;
        
        private const float gravityAcceleration = 9.8f;

        private bool isGrounded;

       
        
        private void Awake()
        {
            _inputController = new InputController();
        }

        private void Update()
        {
            var userActions = _inputController.GetUserInputs();

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded)
            {
                _velocity.y = 0f;
            }

            var verticalMovement = Input.GetAxis("Vertical");
            var horizontalMovement = Input.GetAxis("Horizontal");

            // Dictionary<>

            _velocity.y -= gravityAcceleration * Time.deltaTime;

            characterController.Move(_velocity * Time.deltaTime);
            
            Vector3 movement = transform.TransformDirection(Vector3.right * horizontalMovement + Vector3.forward * verticalMovement);

            characterController.Move(movement * (speed * Time.deltaTime));
        }
    }
}