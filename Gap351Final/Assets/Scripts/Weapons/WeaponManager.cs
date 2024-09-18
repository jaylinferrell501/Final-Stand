using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> WeaponSlots;

    public GameObject CurrentWeaponSlot;

    [Header("Ammo")]
    private int m_revolverAmmo = 0;
    private int m_ak47Ammo = 0;
    private int m_shotGunAmmo = 0;

    [Header("Throwable")]
    private int m_grenades = 0;
    [SerializeField] private float m_throwForce  = 10f;
    [SerializeField] private GameObject m_grenadePrefab;
    [SerializeField] private GameObject m_throwableSpawn;
    private float m_forceMultipler = 0;
    private float m_forceMultiplerLimit = 2;

    private void Awake()
    {
        // Check if there is an existing instance of the SoundManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Set the current weapon slot to the first weapon slot
        CurrentWeaponSlot = WeaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in WeaponSlots)
        {
            if (weaponSlot == CurrentWeaponSlot)
            {
                // Enable the weapon slot
                weaponSlot.SetActive(true);
            }
            else
            {
                // Disable the weapon slot
                weaponSlot.SetActive(false);
            }
        }

        // Check if the player wants to switch weapons
        if (Input.GetKeyDown(KeyCode.Alpha1)) // #1 KEY
        {
            // Switch to the first weapon slot
            SwitchWeaponSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // #2 KEY
        {
            // Switch to the second weapon slot
            SwitchWeaponSlot(1);
        }

        if (Input.GetKey(KeyCode.T))
        {
           
            m_forceMultipler += Time.deltaTime;

            if (m_forceMultipler > m_forceMultiplerLimit)
            {
                m_forceMultipler = m_forceMultiplerLimit;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (m_grenades > 0)
            {
                ThrowGernade();
            }

            m_forceMultipler = 0;
        }
    }

    private void ThrowGernade()
    {
        GameObject gernadePrefab = m_grenadePrefab;

        GameObject throwable = Instantiate(gernadePrefab, m_throwableSpawn.transform.position, Camera.main.transform.rotation );
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (m_throwForce * m_forceMultipler), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().SetHasBeenThrown(true);

        m_grenades -= 1;
    }

    public void PickUpWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponToCurrentSlot(pickedUpWeapon);
    }

    private void AddWeaponToCurrentSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon(pickedUpWeapon);

        // Set the picked up weapon to the current weapon slot
        pickedUpWeapon.transform.SetParent(CurrentWeaponSlot.transform, false);

        // Set the picked up weapon's position and rotation
        WeaponBase weapon = pickedUpWeapon.GetComponent<WeaponBase>();
        pickedUpWeapon.transform.localPosition = new Vector3(weapon.GetWeaponsSpawnPosition().x, weapon.GetWeaponsSpawnPosition().y, weapon.GetWeaponsSpawnPosition().z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.GetWeaponsSpawnRotation().x, weapon.GetWeaponsSpawnRotation().y, weapon.GetWeaponsSpawnRotation().z);

        // Set the picked up weapon to active
        weapon.ActiveWeapon(true);

        // Set the Animator of the weapon to active
        weapon.GetAnimator().enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        // Check if the current weapon slot has a weapon
        if (CurrentWeaponSlot.transform.childCount > 0)
        {
            // Get the weapon to drop
            var WeaponToDrop = CurrentWeaponSlot.transform.GetChild(0).gameObject;

            // Set the weapon to drop to inactive
            WeaponToDrop.GetComponent<WeaponBase>().ActiveWeapon(false);

            // Set the Animator of the weapon to inactive
            WeaponToDrop.GetComponent<WeaponBase>().GetAnimator().enabled = false;

            // Set the weapon to drop's parent to the picked up weapon's parent
            WeaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            WeaponToDrop.transform.localPosition = pickedUpWeapon.transform.position;
            WeaponToDrop.transform.localRotation = pickedUpWeapon.transform.rotation;
        }
    }

    public void SwitchWeaponSlot(int slotIndex)
    {
        // Check if the current weapon slot has a weapon
        if (CurrentWeaponSlot.transform.childCount > 0)
        {
            // Get the current weapon
            WeaponBase currentWeapon = CurrentWeaponSlot.transform.GetChild(0).GetComponent<WeaponBase>();
            currentWeapon.ActiveWeapon(false); // Set the current weapon to inactive
        }

        CurrentWeaponSlot = WeaponSlots[slotIndex]; // Set the current weapon slot to the selected weapon slot

        // Check if the current weapon slot has a weapon
        if (CurrentWeaponSlot.transform.childCount > 0)
        {
            // Get the new weapon
            WeaponBase newWeapon = CurrentWeaponSlot.transform.GetChild(0).GetComponent<WeaponBase>();
            newWeapon.ActiveWeapon(true); // Set the new weapon to active
        }
    }

    public void PickUpAmmo(AmmoBox ammoBox)
    {
        switch (ammoBox.GetAmmoType())
        {
            case AmmoBox.AmmoType.kRevolver:
                m_revolverAmmo += ammoBox.GetAmmoAmount();
                break;
            case AmmoBox.AmmoType.kAk47:
                m_ak47Ammo += ammoBox.GetAmmoAmount();
                break;
            case AmmoBox.AmmoType.kShotGun:
                m_shotGunAmmo += ammoBox.GetAmmoAmount();
                break;
            default:
                Debug.Log("Not an ammo type");
                break;
        }
    }

    public int GetTotalAk47Ammo()
    {
        return m_ak47Ammo;
    }

    public int GetTotalRevolverAmmo()
    {
        return m_revolverAmmo;
    }

    public int GetTotalShotgunAmmo()
    {
        return m_shotGunAmmo;
    }

    public int CheckAmmoLeft(WeaponModels.WeaponModel mThisWeaponModel)
    {
        switch (mThisWeaponModel)
        {
            case WeaponModels.WeaponModel.kRevolver:
                return WeaponManager.Instance.GetTotalRevolverAmmo();
            case WeaponModels.WeaponModel.kAK47:
                return WeaponManager.Instance.GetTotalAk47Ammo();
            case WeaponModels.WeaponModel.kShotgun:
                return WeaponManager.Instance.GetTotalShotgunAmmo();
            default:
                return -1;

        }
    }

    public void DecreaseTotalAmmo(int mBulletsLeft, WeaponModels.WeaponModel mThisWeaponModel)
    {
        switch (mThisWeaponModel)
        {
            case WeaponModels.WeaponModel.kAK47:
                m_ak47Ammo -= mBulletsLeft;
                break;
            case WeaponModels.WeaponModel.kRevolver:
                m_revolverAmmo -= mBulletsLeft;
                break;
            case WeaponModels.WeaponModel.kShotgun:
                m_shotGunAmmo -= mBulletsLeft;
                break;
        }
    }

    public void PickUpThrowable(Throwable mSelectedThrowable)
    {
        switch (mSelectedThrowable.GetThrowableType())
        {
            case Throwable.ThrowableType.kGrenade:
                PickUpGrenade();
                break;
        }
    }

    private void PickUpGrenade()
    {
         m_grenades += 1;
    }

    public int GetAmmoutOfGernades()
    {
        return m_grenades;
    }
}
