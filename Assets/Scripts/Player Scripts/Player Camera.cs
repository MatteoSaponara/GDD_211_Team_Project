using UnityEngine;
using UnityEngine.InputSystem;

namespace game
{
    public class PlayerCamera : MonoBehaviour
    {
        [Tooltip("Position of the player")]
        [SerializeField] private Transform playerBody;
        [Tooltip("Sensitivity of player input")]
        [SerializeField] private float sensitivity = 100f;

        // Vertical rotation of the camera
        private float xRotation = 0f;
        private Vector2 lookInput;

        // Gets camera vector from player input
        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                lookInput = context.ReadValue<Vector2>();
                // Debug.Log("LOOK: " + lookInput);
            }
            else if (context.canceled)
            {
                lookInput = Vector2.zero;
            }
        }

        // Locks cursor and turns off visibility
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Takes player input and moves the camera
        void Update()
        {
            float mouseX = lookInput.x * sensitivity * Time.deltaTime;
            float mouseY = lookInput.y * sensitivity * Time.deltaTime;

            // Vertical rotation (camera only)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal rotation (player body)
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
