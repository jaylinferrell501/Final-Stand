using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GlobalReferences class
// This class is used to store global references
public class GlobalReferences : MonoBehaviour
{
    // Singleton instance
    public static GlobalReferences Instance { get; set; }

    public GameObject g_grenadeExplosionEffect;

    public GameObject g_bulletImpactEffect;

    public GameObject g_player;

    public GameObject g_waypointCluster;

    public GameObject g_bloodSprayEffect;

    public int g_WaveNumber;

    private void Awake()
    {
        // Check if there is an existing instance of the GlobalReferences
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
