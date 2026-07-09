using UnityEngine;

namespace game
{
    public class ShockwaveWall : MonoBehaviour
    {
        [Tooltip("Shockwave script of the parent Shockwave.")]
        [SerializeField] private Shockwave shockwave;

        // Increases size of shockwave visual
        void Update()
        {
            if (shockwave == null)
                return;

            float radius = shockwave.CurrentRadius;

            float scale = radius * 2f;

            transform.localScale = new Vector3(scale, 1f, scale);

            // Keeps shockwave grounded
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, 0.05f, pos.z);
        }
    }
}