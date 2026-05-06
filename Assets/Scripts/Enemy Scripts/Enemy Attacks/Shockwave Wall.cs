using UnityEngine;

namespace game
{
    public class ShockwaveWall : MonoBehaviour
    {
        [SerializeField] private Shockwave shockwave;

        void Update()
        {
            if (shockwave == null)
                return;

            float radius = shockwave.CurrentRadius;

            float scale = radius * 2f;

            transform.localScale = new Vector3(scale, 0.2f, scale);

            // Keeps shockwave grounded
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, 0.05f, pos.z);
        }
    }
}