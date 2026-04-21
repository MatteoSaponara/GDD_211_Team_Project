using game;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Shooting : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Bullet trail visualizer")]
        [SerializeField] private LineRenderer lr;
        [Tooltip("Camera position where bullets are fired from")]
        [SerializeField] private Transform camTrans;

        [Header("Shooting Mechanics")]
        [Tooltip("The damage each bullet does")]
        [SerializeField] private float damage;

        private List<LineRenderer> lineRenderers = new List<LineRenderer>();

        public void OnShoot(InputAction.CallbackContext context)
        {
            Debug.Log($"Shooting {context.performed}");
            if (!GameManager.Instance.isPaused)
            {
                Shoot();
            }
            
        }

        public void Shoot()
        {
            if (Physics.Raycast(camTrans.position, camTrans.forward, out RaycastHit hit))
            {
                // Apply damage / effect
                // Debug.Log("Raycast hit: " + hit);
                if (hit.collider.TryGetComponent<Enemy>(out var target))
                {
                    Debug.Log("Enemy hit: " + target);
                    target.TakeDamage(damage);
                }


                //Draw line
                LineRenderer newLR = Instantiate(lr, transform.position, Quaternion.identity);
                newLR.SetPositions(new Vector3[]
                {
                        camTrans.position + Vector3.down * 0.1f,
                        hit.point
                });
                lineRenderers.Add(newLR);
            }
        }

        private void FixedUpdate()
        {
            foreach (LineRenderer renderer in lineRenderers)
            {
                // Fade out lines / destroy
                if (renderer != null)
                {
                    Destroy(renderer.gameObject, 0.5f);
                }
            }
        }
    }
}