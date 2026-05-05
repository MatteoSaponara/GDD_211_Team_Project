using game; // Asked google search result AI how to make coroutine for reloading and firerate. Asked ChatGPT for proper trail destruction
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Game
{
    public class Shooting : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Bullet trail visualizer")]
        [SerializeField] private GameObject lr;
        [Tooltip("Position of muzzle where bullets visuals are fired from")]
        [SerializeField] private Transform muzzle;
        [Tooltip("Camera position where raycasts come from")]
        [SerializeField] private Transform camTrans;

        [Header("Shooting Mechanics")]
        [Tooltip("The damage each bullet does")]
        [SerializeField] private float damage;
        [Tooltip("The maximum amount ammo loaded")]
        [SerializeField] private float maxAmmo;
        [Tooltip("How long it takes to reload a shell")]
        [SerializeField] private float reloadTime;
        [Tooltip("How many pellets are in a shell")]
        [SerializeField] private float pelletsPerShot;
        [Tooltip("Distance pellets deviate")]
        [SerializeField] private float inaccuracyDeviation;
        [Tooltip("Cooldown between shots in seconds")]
        [SerializeField] private float fireRate;
        [Tooltip("Pellet trail length")]
        [Range(0f, 1f)]
        [SerializeField] private float pelletTrail = 0.15f;

        // Amount of ammo currently loaded
        private float currentAmmo;
        // Determines if gun is reloading
        private bool reloading = false;
        // Determines if reloading coroutine is running
        private bool reloadCouroutine = false;
        // In game time of when gun was last fired
        private float timeOfLastShot = 0;

        private List<TrailData> lineRenderers = new List<TrailData>();

        public void OnShoot(InputAction.CallbackContext context)
        {
            Debug.Log($"Shooting {context.performed}");
            if (!GameManager.instance.isPaused && currentAmmo > 0 && Time.time - timeOfLastShot > fireRate)
            {
                Shoot();
                timeOfLastShot = Time.time;
            }
            else
            {
                // Debug.Log("If game is not paused, ammo is 0");
            }
            
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            Debug.Log($"Reloading {context.performed}");
            if (!GameManager.instance.isPaused && currentAmmo < maxAmmo)
            {
                Reload();
            }
        }

        public void Shoot()
        {
            reloading = false;
            currentAmmo--; // Decreases ammo
            Debug.Log("Ammo spent");

            for (int i = 0; i < pelletsPerShot /* -1 */; i++) // Uncomment code for consistent middle pellet
            {
                if (Physics.Raycast(camTrans.position, GetShootingDirection(), out RaycastHit hit))
                {
                    // Apply damage / effect
                    // Debug.Log("Raycast hit: " + hit);
                    if (hit.collider.TryGetComponent<Enemy>(out var target))
                    {
                        Debug.Log("Enemy hit: " + target);
                        target.TakeDamage(damage);
                        
                    } 
                    // Draws line
                    CreateTrail(hit.point);

                }
                else
                {
                    CreateTrail(camTrans.position + GetShootingDirection() * 100f);
                }

            }
            // Code for consistent middle pellet
            /* if (Physics.Raycast(camTrans.position, camTrans.direction, out RaycastHit hit)) {
             *      if (hit.collider.TryGetComponent<Enemy>(out var target))
                    {
                        Debug.Log("Enemy hit: " + target);
                        target.TakeDamage(damage);
                    }
                    //Draw line
                    CreateTrail(hit.point);
             * }
             * else
               {
                   CreateTrail(camTrans.position + GetShootingDirection() * 100f);
               }
            */
        }

        // Sets reloading to true
        public void Reload()
        {
            reloading = true;
        }

        private void Start()
        {
            currentAmmo = maxAmmo;
        }

        private void Update()
        {
            if (reloading && !reloadCouroutine)
            {
                Debug.Log("Starting reload couritine");
                StartCoroutine(Reloader());
            }
        }

        private void FixedUpdate() 
        {
            for (int i = lineRenderers.Count - 1; i >= 0; i--)
            {
                TrailData data = lineRenderers[i];

                if (data.lr == null)
                {
                    lineRenderers.RemoveAt(i);
                    continue;
                }

                data.lifeTimer += Time.fixedDeltaTime;
                float t = data.lifeTimer / data.lifeTime;

                // Fade out alpha
                Color newStart = data.startColor;
                Color newEnd = data.endColor;

                newStart.a = Mathf.Lerp(data.startColor.a, 0f, t);
                newEnd.a = Mathf.Lerp(data.endColor.a, 0f, t);

                data.lr.startColor = newStart;
                data.lr.endColor = newEnd;

                // Shrinks width
                data.lr.widthMultiplier = Mathf.Lerp(1f, 0f, t);

                if (data.lifeTimer >= data.lifeTime)
                {
                    Destroy(data.lr.gameObject);
                    lineRenderers.RemoveAt(i);
                }
            }
        }

        // Reloads ammo every reloadTime seconds
        IEnumerator Reloader()
        {
            reloadCouroutine = true;
            while(reloading && currentAmmo < maxAmmo)
            {
                yield return new WaitForSeconds(reloadTime);
                currentAmmo++;
                Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);
            }
            reloadCouroutine = false;
        }

        // Gives shooting direction
        private Vector3 GetShootingDirection()
        {
            Vector3 targetPos = camTrans.position + camTrans.forward;
            // POSSIBLE BUG: Might need to change z to just targetPos.z
            targetPos = new Vector3 (targetPos.x + Random.Range(-inaccuracyDeviation, inaccuracyDeviation), targetPos.y + Random.Range(-inaccuracyDeviation, inaccuracyDeviation), targetPos.z + Random.Range(-inaccuracyDeviation, inaccuracyDeviation));
            Vector3 direction = targetPos - camTrans.position;
            return direction.normalized;
        }

        // Creates bullet trail line from muzzle
        private void CreateTrail(Vector3 end)
        {
            LineRenderer newLR = Instantiate(lr).GetComponent<LineRenderer>();
            newLR.SetPositions(new Vector3[2] { muzzle.position, end });

            TrailData data = new TrailData();
            data.lr = newLR;
            data.lifeTimer = 0f;
            data.lifeTime = pelletTrail; // short shotgun trail

            data.startColor = newLR.startColor;
            data.endColor = newLR.endColor;

            lineRenderers.Add(data);
        }

        private class TrailData // Helper class for destroying bullet trails
        {
            public LineRenderer lr;
            public float lifeTimer;
            public float lifeTime;

            public Color startColor;
            public Color endColor;
        }
    }
}