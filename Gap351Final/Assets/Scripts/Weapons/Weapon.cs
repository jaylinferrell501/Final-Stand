using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBase : MonoBehaviour
{

    // ---------------------------------------- Shooting Mode ----------------------------------------
    public enum ShootingMode { SemiAuto, Burst, FullAuto }; // Shooting mode
    private Animator m_animator; // Animator of the weapon // Other scripts can access this m_animator.
    [SerializeField] private Vector3 m_weaponsSpawnPosition; // Weapon spawn position
    [SerializeField] private Vector3 m_weaponsSpawnRotation; // Weapon spawn rotation
    [SerializeField] private bool m_isActiveWeapon;

    [Header("Shooting Mode")]
    [SerializeField] private ShootingMode m_currentShootingMode; // Current shooting mode

    [Header("Weapon Settings")]
    [SerializeField] private Texture m_weaponImage;   // Weapon image
    [SerializeField] private GameObject m_bulletPrefab;   // Bullet prefab
    [SerializeField] private Transform m_firePoint;   // Fire point of the weapon
    // ---------------------------------------- SHOOTING ----------------------------------------
    [SerializeField] private bool m_isShooting, m_readyToShoot; // Is the player shooting, is the player ready to shoot
    private bool m_allowReset = true; // Allow the player to reset the weapon
    [SerializeField] private float m_timeBetweenShots = 2f; // Time between shots
    [SerializeField] private WeaponModels.WeaponModel m_thisWeaponModel; // The weapon model
    [SerializeField] private float m_thisWeaponDamage; // The weapon damage

    // Reloading
    [SerializeField] private bool m_isReloading; // Is the player reloading
    [SerializeField] private float m_reloadTime = 1.5f; // Reload time
    [SerializeField] private int m_bulletsPerMag = 12; // Bullets per magazine
    [SerializeField] private int m_bulletsLeft; // Bullets left

    // Muzzle Flash
    [SerializeField] private GameObject m_muzzleFlashEffectPrefab; // Muzzle flash prefab

    [Header("Bullet Settings")]
    [SerializeField] private float m_bulletVelocity = 30f;   // Bullet velocity
    [SerializeField] private float m_bulletPrefablifeTime = 3f;   // Bullet life time

    // ---------------------------------------- Burst ----------------------------------------
    [SerializeField] private int m_bulletsPerBurst = 3; // Bullets per burst
    [SerializeField] private int m_burstBulletsLeft; // how many bullets are left in the burst

    // ---------------------------------------- SPREAD ----------------------------------------
    private float m_spreadIntensity;
    [SerializeField] private float m_hipSpreadIntensity;
    [SerializeField] private float m_adsSpreadIntensity;

    // ---------------------------------------- ASD -------------------------------------------
    private bool m_IsADS;

    private Outline m_outline;



    private void Awake()
    {
        m_readyToShoot = true;
        m_burstBulletsLeft = m_bulletsPerBurst;
        m_animator = GetComponent<Animator>();

        // Set the bullets left to the bullets per mag
        m_bulletsLeft = m_bulletsPerMag;

        m_spreadIntensity = m_hipSpreadIntensity;

        m_outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the weapon is active
        if (!m_isActiveWeapon)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            return;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_animator.SetBool("EnterADS", true);
            m_spreadIntensity = m_adsSpreadIntensity;
            m_IsADS = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("ExitADS", true);
            m_spreadIntensity = m_hipSpreadIntensity;
            m_IsADS = false;
        }




        // Check if the player wants to switch weapons
        m_outline.enabled = false;

        // Check if the player is shooting and has bullets left
        if (m_bulletsLeft == 0 && m_isShooting)
        {
            SoundManager.Instance.GunEmptySound.Play();
        }

        // Check shooting mode
        if (m_currentShootingMode == ShootingMode.FullAuto)
        {
            // Hold the mouse button to shoot
            m_isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (m_currentShootingMode == ShootingMode.SemiAuto)
        {
            // Click the mouse button to shoot
            m_isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        else if (m_currentShootingMode == ShootingMode.Burst)
        {
            // Click the mouse button to shoot
            m_isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Check if the player clicked the reload button
        if (Input.GetKeyDown(KeyCode.R) && m_bulletsLeft < m_bulletsPerMag && !m_isReloading && WeaponManager.Instance.CheckAmmoLeft(m_thisWeaponModel) > 0)
        {
            Reload();
        }

        // Auto reload
        if (m_readyToShoot && !m_isShooting && !m_isReloading && m_bulletsLeft <= 0 && WeaponManager.Instance.CheckAmmoLeft(m_thisWeaponModel) > 0)
        {
            Reload();
        }

        if (m_readyToShoot && m_isShooting && m_bulletsLeft > 0 && !m_isReloading )
        {
            m_burstBulletsLeft = m_bulletsPerBurst;

            // Shoot the weapon
            ShootWeapon();
        }
    }

    private void ShootWeapon()
    {

        // Decrease the bullets left
        m_bulletsLeft--;

        // Muzzle flash
        m_muzzleFlashEffectPrefab.GetComponent<ParticleSystem>().Play();

        // Play the shoot animation
        if (m_IsADS)
        {
            m_animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            m_animator.SetTrigger("RECOIL");
        }
            

        // Play the shoot sound
        SoundManager.Instance.PlayShootingSoundFor(m_thisWeaponModel);

        // Set the ready to shoot to false
        m_readyToShoot = false;

        // The shooting direction
        Vector3 shootDirection = CalculateDirectionAndSpread().normalized;

        // Create the bullet
        GameObject bullet = Instantiate(m_bulletPrefab, m_firePoint.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().SetBulletDamage(m_thisWeaponDamage);

        // Set the forward of the bullet to the shoot direction
        bullet.transform.forward = shootDirection;

        // Get the rigidbody of the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Add force to the bullet
        rb.AddForce(shootDirection * m_bulletVelocity, ForceMode.Impulse);

        // Destroy the bullet after the life time
        Destroy(bullet, m_bulletPrefablifeTime);

        // Check if we are done shooting
        if (m_allowReset)
        {
            Invoke("ResetShot", m_timeBetweenShots);
            m_allowReset = false;
        }

        // Burst mode
        if (m_currentShootingMode == ShootingMode.Burst && m_burstBulletsLeft > 1) // we already shot one bullet
        {
            m_burstBulletsLeft--;
            Invoke("ShootWeapon", m_timeBetweenShots);
        }
    }

    private void Reload()
    {
        // Play the reload animation
        m_animator.SetTrigger("RELOAD");

        // Play the reload Sound
        SoundManager.Instance.PlayReloadSoundFor(m_thisWeaponModel);

        // Set the reloading to true
        m_isReloading = true;

        // Invoke the reload finish
        Invoke("ReloadFinish", m_reloadTime);

    }

    private void ReloadFinish()
    {
        if (WeaponManager.Instance.CheckAmmoLeft(m_thisWeaponModel) > m_bulletsPerMag)
        {
            m_bulletsLeft = m_bulletsPerMag;
            WeaponManager.Instance.DecreaseTotalAmmo(m_bulletsLeft, m_thisWeaponModel);
        }
        else
        {
            m_bulletsLeft = WeaponManager.Instance.CheckAmmoLeft(m_thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(m_bulletsLeft, m_thisWeaponModel);
        }

        // Set the reloading to false
        m_isReloading = false;
    }

    private void ResetShot()
    {
        m_readyToShoot = true;
        m_allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        // Shooting from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Get the point where the ray hit
        Vector3 targetPoint;

        // If the ray hit something
        targetPoint = Physics.Raycast(ray, out hit) ? hit.point :
            // If the ray didn't hit anything
            ray.GetPoint(100);

        // Calculate the direction from the fire point to the target point
        Vector3 direction = targetPoint - m_firePoint.position;

        // Calculate the spread
        float z = Random.Range(-m_spreadIntensity, m_spreadIntensity);
        float y = Random.Range(-m_spreadIntensity, m_spreadIntensity);

        // Return the direction with the spread
        return direction + new Vector3(0, y, z);
    }

    public Vector3 GetWeaponsSpawnPosition()
    {
        return m_weaponsSpawnPosition;
    }

    public Vector3 GetWeaponsSpawnRotation()
    {
        return m_weaponsSpawnRotation;
    }

    public void ActiveWeapon(bool value)
    {
        m_isActiveWeapon = value;
    }

    public bool ThisIsTheActiveWeapon()
    {
        return m_isActiveWeapon;
    }

    public Animator GetAnimator()
    {
        return m_animator;
    }

    public float GetBulletsLeft()
    {
        return m_bulletsLeft;
    }

    public float GetBulletsPerBurst()
    {
        return m_bulletsPerBurst;
    }

    public float GetMagSize()
    {
        return m_bulletsPerMag;
    }

    public Texture GetWeaponImage()
    {
        return m_weaponImage;
    }

    public WeaponModels.WeaponModel GetWeaponModel()
    {
        return m_thisWeaponModel;
    }
}
