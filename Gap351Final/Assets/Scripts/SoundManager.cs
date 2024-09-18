using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Refractor how this works
public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager Instance { get; set; }

    public AudioSource RevolverGunSound;
    public AudioSource RevolverGunReloadSound;

    public AudioSource AK47GunSound;
    public AudioSource AK47GunReloadSound;

    public AudioSource ShotGunSound;
    public AudioSource ShotGunReloadSound;

    public AudioSource GunEmptySound;

    public AudioSource GernadeSound;

    public AudioSource ZombieWalking;
    public AudioSource ZombieChase;
    public AudioSource ZombieAttack;
    public AudioSource ZombieHurt;
    public AudioSource ZombieDeath;

    public AudioSource PlayerHurt;
    public AudioSource PlayerDie;

    public AudioSource DeathMusic;

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

    public void PlayShootingSoundFor(WeaponModels.WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModels.WeaponModel.kRevolver:
                RevolverGunSound.Play();
                break;
            case WeaponModels.WeaponModel.kAK47:
                AK47GunSound.Play();
                break;
            case WeaponModels.WeaponModel.kShotgun:
                ShotGunSound.Play();
                break;
            default:
                // Default sound
                break;
        }
    }

    public void PlayReloadSoundFor(WeaponModels.WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModels.WeaponModel.kRevolver:
                RevolverGunReloadSound.Play();
                break;
            case WeaponModels.WeaponModel.kAK47:
                AK47GunReloadSound.Play();
                break;
            case WeaponModels.WeaponModel.kShotgun:
                ShotGunReloadSound.Play();
                break;
            default:
                // Default sound
                break;
        }
    }
}
