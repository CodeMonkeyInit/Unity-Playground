using UnityEngine;

namespace UnityTemplateProjects
{
    public class UserCameraController : MonoBehaviour
    {
        private CameraController _cameraController;

        public Transform playerTransform;
        
        private float xRotation;
        
        private void Awake()
        {
            _cameraController = new CameraController();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            var cameraInput = _cameraController.GetCameraInput();


            xRotation -= cameraInput.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            playerTransform.Rotate(Vector3.up * cameraInput.x);
        }
    }
}