using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlackout : MonoBehaviour
{
    [SerializeField] private Image m_fadeBlackoutImage;
    [SerializeField] private float m_fadeBlackoutDuration = 7.0f;

    public void StartFade()
    {
        StartCoroutine(FadeOut());
        m_fadeBlackoutImage.gameObject.SetActive(true);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = m_fadeBlackoutImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); // Black with alpha 1.

        while (timer < m_fadeBlackoutDuration)
        {
            // Interpolate the color between start and end over time.
            m_fadeBlackoutImage.color = Color.Lerp(startColor, endColor, timer / m_fadeBlackoutDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is completely black at the end.
        m_fadeBlackoutImage.color = endColor;
    }
}
