using UnityEngine;

namespace game
{
    public class PelletTrail : MonoBehaviour // Assisted with AI
    {
        // Raycast endpoint
        private Vector3 target;
        // Speed of pellet trail
        private float speed;

        // Sets up pellet trail
        public void Initialize(Vector3 start, Vector3 targetPosition, float travelTime)
        {
            transform.position = start;
            target = targetPosition;

            speed = Vector3.Distance(start, targetPosition) / travelTime;
        }

        // Moves pellet renderer
        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (transform.position == target)
            {
                Destroy(gameObject);
            }
        }
    }
}
