using UnityEngine;
using UnityEngine.InputSystem;

namespace game
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the player's Character Controller.")]
        [SerializeField] private CharacterController controller;

        [Header("Movement Stats")]
        [Tooltip("Movement speed of the player")]
        [SerializeField] private float speed = 5f;
        [Tooltip("Jump height of the player.")]
        [SerializeField] private float jumpHeight = 2f;
        [Tooltip("Downward acceleration of gravity on the player.")]
        [SerializeField] private float gravity = -9.8f;

        private Vector2 moveInput;
        // Current velocity of the player
        private Vector3 velocity;

        // Gets movement vector from player input
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            // Debug.Log($"Move Input: {moveInput}");
        }

        // Makes the player jump
        // NOTE: It could be good to make the jump code a seperate function
        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log($"Jumping {context.performed} - Is Grounded: {controller.isGrounded}");
            if (context.performed && controller.isGrounded)
            {
                // INSERT: Grunt sound effect (because it would be funny)
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

        // Moves the player
        void Update()
        {
            // WASD player movement
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(move * speed * Time.deltaTime);

            // Small downward force to keep the player grounded
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Pull of gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}

