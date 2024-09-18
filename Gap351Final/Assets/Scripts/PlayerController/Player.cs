using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_health = 100f;
    [SerializeField] private bool m_isDead;
    [SerializeField] private GameObject m_bloodyScreen;

    public void TakeDamage(float damageAmount)
    {
        m_health -= damageAmount;

        if (m_health <= 0)
        {
            m_health = 0;

            PlayerDead();
            m_isDead = true;
        }
        else
        {
            StartCoroutine(DisplayBloodyScreenEffect());
            SoundManager.Instance.PlayerHurt.Play();
        }
    }

    private void PlayerDead()
    {
        int waveSurvived = GlobalReferences.Instance.g_WaveNumber - 1;

        if (waveSurvived > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurvived);
        }

        // Cache the parent transform
        Transform parentTransform = transform.parent;

        SoundManager.Instance.PlayerDie.Play();

        // Check if parentTransform is not null
        if (parentTransform != null)
        {
            // Disable MouseMovement and PlayerMovement components on the parent
            MouseMovement mouseMovement = parentTransform.GetComponentInChildren<MouseMovement>();
            if (mouseMovement != null)
            {
                mouseMovement.enabled = false;
            } 
            PlayerMovment playerMovement = parentTransform.GetComponentInChildren<PlayerMovment>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // Enable the Animator component on the parent
            Animator animator = parentTransform.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                parentTransform.GetComponent<ScreenBlackout>().StartFade();
                SoundManager.Instance.DeathMusic.PlayDelayed(1f);
            }
            else
            {
                Debug.LogWarning("Animator component not found on parent or its children.");
            }

            StartCoroutine(ReturnToMainMenu());


        }
        else
        {
            Debug.LogWarning("Parent transform not found.");
        }
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator DisplayBloodyScreenEffect()
    {
        if (!m_bloodyScreen.activeInHierarchy)
        {
            m_bloodyScreen.SetActive(true);
        }

        var image = m_bloodyScreen.GetComponentInChildren<RawImage>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame.
        }

        if (m_bloodyScreen.activeInHierarchy)
        {
            m_bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ZombieHandHitBox")
        {

            if (!m_isDead)
                TakeDamage(other.gameObject.GetComponent<ZombieHandHitBox>().GetDamage());
        }
    }

    public float GetHealth()
    {
        return m_health;
    }
}
