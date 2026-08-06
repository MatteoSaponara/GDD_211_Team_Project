using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;

namespace game
{
    public class Shooting : MonoBehaviour // Asked google search result AI how to make coroutine for reloading and firerate. Asked ChatGPT for proper trail destruction
    {
        [Header("References")]
        [Tooltip("Position of muzzle where bullets visuals are fired from")]
        [SerializeField] private Transform muzzle;
        [Tooltip("Camera position where raycasts come from")]
        [SerializeField] private Transform camTrans;
        [Tooltip("Ammo UI Text")]
        [SerializeField] private TextMeshProUGUI ammoText;
        [Tooltip("Prefab used to visualize shotgun pellets")]
        [SerializeField] private GameObject pelletTrailPrefab;

        [Header("Shooting Mechanics")]
        [Tooltip("The damage each bullet does")]
        [SerializeField] private float damage;
        [Tooltip("The maximum amount ammo loaded")]
        [SerializeField] private float maxAmmo;
        [Tooltip("How long it takes to reload a shell")]
        [SerializeField] private float reloadTime;
        [Tooltip("How many pellets are in a shell")]
        [SerializeField] private float pelletsPerShot;
        [Tooltip("Max deviation of pellet spread")]
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
            UpdateAmmoUI();
            GameManager.instance.SoundManager.PlaySound(SoundType.GUNSHOT); // Gunshot sound effect
            Debug.Log("Ammo spent");

            for (int i = 0; i < pelletsPerShot - 1; i++) // Uncomment -1 code for consistent middle pellet
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
                    CreatePelletTrail(hit.point);

                }
                else
                {
                    CreatePelletTrail(camTrans.position + GetShootingDirection() * 100f);
                }

            }
            // Code for consistent middle pellet
             if (Physics.Raycast(camTrans.position, camTrans.forward, out RaycastHit midHit)) {
                if (midHit.collider.TryGetComponent<Enemy>(out var target))
                {
                    Debug.Log("Enemy hit: " + target);
                    target.TakeDamage(damage);
                }
                //Draw line
                CreatePelletTrail(midHit.point);
            }
             else
               {
                CreatePelletTrail(camTrans.position + GetShootingDirection() * 100f);
            }
            
        }

        // Sets reloading to true
        public void Reload()
        {
            reloading = true;
        }

        private void Start()
        {
            currentAmmo = maxAmmo;
            UpdateAmmoUI();
        }

        private void Update()
        {
            if (reloading && !reloadCouroutine)
            {
                Debug.Log("Starting reload couritine");
                StartCoroutine(Reloader());
            }
        }

        // Reloads ammo every reloadTime seconds
        IEnumerator Reloader()
        {
            reloadCouroutine = true;
            while(reloading && currentAmmo < maxAmmo)
            {
                yield return new WaitForSeconds(reloadTime);
                if (reloading)
                {
                    currentAmmo++;
                    UpdateAmmoUI();
                    Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);
                }
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

        // Creates bullet/pellet trail line from muzzle
        private void CreatePelletTrail(Vector3 end)
        {
            GameObject pellet = Instantiate(pelletTrailPrefab, muzzle.position, Quaternion.identity);

            PelletTrail pelletTrail = pellet.GetComponent<PelletTrail>();

            pelletTrail.Initialize(muzzle.position, end, 0.05f); // Change float to change how long the trail lasts
        }

        private void UpdateAmmoUI()
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }

        public void ChangeFireRate()
        {
            fireRate = fireRate * 2;
            Debug.Log("Firerate changed: " + fireRate);
        }
    }
}