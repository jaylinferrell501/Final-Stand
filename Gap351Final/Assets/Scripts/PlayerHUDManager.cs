using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    public static PlayerHUDManager Instance { get; set; }

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI m_ammoTextUi;
    [SerializeField] private TextMeshProUGUI m_throwableAmountTextUi;
    [SerializeField] private RawImage m_activeWeapon;
    [SerializeField] private TextMeshProUGUI m_playerHealth;

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

    public void Update()
    {
        WeaponBase activeWeapon = WeaponManager.Instance.CurrentWeaponSlot.GetComponentInChildren<WeaponBase>();
        Color color = m_activeWeapon.color;

        GameObject player = GlobalReferences.Instance.g_player;
        m_playerHealth.text = $"Health: {player.gameObject.GetComponentInChildren<Player>().GetHealth()}";

        if (activeWeapon != null)
        {
            m_ammoTextUi.text = $"{activeWeapon.GetBulletsLeft() / activeWeapon.GetBulletsPerBurst()} / {WeaponManager.Instance.CheckAmmoLeft(activeWeapon.GetWeaponModel())}";
            m_activeWeapon.texture = activeWeapon.GetWeaponImage();
            color.a = 1f; // Set the alpha to 1 (fully opaque)
            m_activeWeapon.color = color;
        }
        else
        {
            color.a = 0f; 
            m_activeWeapon.color = color;
            m_ammoTextUi.text = "0/0";
        }

        m_throwableAmountTextUi.text = $"{WeaponManager.Instance.GetAmmoutOfGernades()}";
    }
}