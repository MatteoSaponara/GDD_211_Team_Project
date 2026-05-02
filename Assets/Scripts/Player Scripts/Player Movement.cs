using UnityEngine;
using UnityEngine.InputSystem;

namespace game
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.8f;

        private Vector2 moveInput;
        private Vector3 velocity;

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            // Debug.Log($"Move Input: {moveInput}");
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log($"Jumping {context.performed} - Is Grounded: {controller.isGrounded}");
            if (context.performed && controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GameManager.instance.player = this.gameObject;
            Debug.Log("Player movement start: " + GameManager.instance.player);
            controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            // --- MOVEMENT ---
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(move * speed * Time.deltaTime);

            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // small downward force to keep grounded
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}

