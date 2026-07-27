using UnityEngine;

namespace game
{
    public class Billboard : MonoBehaviour // Assisted with AI
    {
        // Reference to the main camera
        private Camera cam;

        // Gets the main camera
        private void Start()
        {
            cam = Camera.main;
        }

        // Makes object face towards the camera
        private void LateUpdate()
        {
            transform.forward = cam.transform.forward;
        }
    }
}
