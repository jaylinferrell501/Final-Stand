using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class ZombieSpawnManager : MonoBehaviour
{
    [SerializeField] private float m_healthModifier = 5;
    [SerializeField] private int m_timeBeforeDespawnAfterDeath = 6;
    [SerializeField] private int m_initalZombiesPerWave = 5;
    [SerializeField] private int m_zombiesPerWaveMultipler = 2;
    private int m_currentZombiesPerWave;

    [SerializeField] private float m_spawnDelay = 5.0f; // Delay between spawns

    [SerializeField] private float m_waveCooldown = 10.0f; // Time in seconds between waves;
    private int m_currentWave = 0;

    private bool m_inCooldown;
    private float m_cooldownCounter = 0;

    [SerializeField] private List<Enemy> m_currentZombiesAlive;
    [SerializeField] private GameObject m_zombiePrefab;

    [SerializeField] private TextMeshProUGUI m_waveOverUi;
    [SerializeField] private TextMeshProUGUI m_cooldownCounterUi;
    [SerializeField] private TextMeshProUGUI m_waveCount;

    private void Start()
    {
        m_currentZombiesPerWave = m_initalZombiesPerWave;

        GlobalReferences.Instance.g_WaveNumber = m_currentWave;

        StartNextWave();
    }

    private void StartNextWave()
    {

        // Clear list
        m_currentZombiesAlive.Clear();

        // Increase wave
        m_currentWave++;

        GlobalReferences.Instance.g_WaveNumber = m_currentWave;

        // Spawn zombies
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < m_currentZombiesPerWave; i++)
        {
            // Make a random offset with a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Instantiate the zombie
            var zombie = Instantiate(m_zombiePrefab, spawnPosition, Quaternion.identity);

            // Get the Enemy Script
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            if (m_currentWave != 1)
                enemyScript.IncreaseEnemyHealthBy(m_healthModifier);

            // Add zombie to the list
            m_currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(m_spawnDelay);
        }
    }

    private void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (var zombie in m_currentZombiesAlive)
        {
            if (zombie.IsDead())
                zombiesToRemove.Add(zombie);
        }

        // Actually remove all dead zombies
        foreach (var zombie in zombiesToRemove)
        {
            if (zombie.IsDead())
            {
                m_currentZombiesAlive.Remove(zombie);
                Destroy(zombie.gameObject, m_timeBeforeDespawnAfterDeath);
            }
                
        }

        zombiesToRemove.Clear();

        // Start Cooldown if all zombies are dead
        if (m_currentZombiesAlive.Count == 0 && !m_inCooldown)
        {
            // Start cooldown for next wave
            StartCoroutine(WaveCooldown());
        }

        // Run the cooldown counter
        if (m_inCooldown)
        {
            m_cooldownCounter -= Time.deltaTime;
        }
        else
        {
            m_cooldownCounter = m_waveCooldown;
        }

        m_cooldownCounterUi.text = m_cooldownCounter.ToString("F0");

        m_waveCount.text = $"Wave: {m_currentWave}";
    }

    private IEnumerator WaveCooldown()
    {
        m_inCooldown = true;

        m_waveOverUi.gameObject.SetActive(true);

        m_healthModifier += m_healthModifier;

        yield return new WaitForSeconds(m_waveCooldown);

        m_inCooldown = false;
        m_waveOverUi.gameObject.SetActive(false);

        m_currentZombiesPerWave *= m_zombiesPerWaveMultipler;

        StartNextWave();
    }
}
