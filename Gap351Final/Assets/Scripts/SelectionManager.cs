using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

   [SerializeField] private WeaponBase m_selectedWeapon = null;
   [SerializeField] private AmmoBox m_selectedAmmoBox = null;
   [SerializeField] private Throwable m_selectedThrowable = null;

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

    private void Update()
    {
        // Raycast from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f)) // If the ray hits something
        {
            // Get the object that was hit
            GameObject objectHit = hit.transform.gameObject;

            // Check if the object hit is a weapon and is not the active weapon
            if (objectHit.GetComponent<WeaponBase>() && !objectHit.GetComponent<WeaponBase>().ThisIsTheActiveWeapon())
            {
                // Disable the outline of no longer selected item
                if (m_selectedWeapon)
                {
                    m_selectedWeapon.GetComponent<Outline>().enabled = false;
                }


                // Set the selected weapon to the weapon hit
                m_selectedWeapon = objectHit.GetComponent<WeaponBase>();

                // Enable the weapon's outline
                m_selectedWeapon.GetComponent<Outline>().enabled = true;

                // Check if the player wants to pick up the weapon
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpWeapon(objectHit.gameObject);
                }
            }
            else
            {
                if (m_selectedWeapon)
                {
                    // If the object hit is not a weapon, deselect the weapon
                    m_selectedWeapon.GetComponent<Outline>().enabled = false;
                }

            }

            // Check if the object hit is a AmmoBox
            if (objectHit.GetComponent<AmmoBox>())
            {
                // Disable the outline of no longer selected item
                if (m_selectedAmmoBox)
                {
                    m_selectedAmmoBox.GetComponent<Outline>().enabled = false;
                }

                // Set the selected AmmoBox to the weapon hit
                m_selectedAmmoBox = objectHit.GetComponent<AmmoBox>();

                // Enable the AmmoBox outline
                m_selectedAmmoBox.GetComponent<Outline>().enabled = true;

                // Check if the player wants to pick up the weapon
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpAmmo(m_selectedAmmoBox);
                    Destroy(objectHit.gameObject);
                }
            }
            else
            {
                if (m_selectedAmmoBox)
                {
                    // If the object hit is not a weapon, deselect the weapon
                    m_selectedAmmoBox.GetComponent<Outline>().enabled = false;
                }

            }

            // Check if the object hit is a Throwable
            if (objectHit.GetComponent<Throwable>())
            {
                // Disable the outline of no longer selected item
                if (m_selectedThrowable)
                {
                    m_selectedThrowable.GetComponent<Outline>().enabled = false;
                }

                m_selectedThrowable = objectHit.GetComponent<Throwable>();

                // Enable the Throwable outline
                m_selectedThrowable.GetComponent<Throwable>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpThrowable(m_selectedThrowable);
                    Destroy(objectHit.gameObject);
                }
            }
            else
            {
                if (m_selectedThrowable)
                {
                    // If the object hit is not a weapon, deselect the weapon
                    m_selectedThrowable.GetComponent<Outline>().enabled = false;
                }

            }
        }
    }
}
