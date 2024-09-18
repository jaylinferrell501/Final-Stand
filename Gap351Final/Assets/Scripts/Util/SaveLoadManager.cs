using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    private string m_highScorekey = "BestWaveSavedValue";

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

        DontDestroyOnLoad(this);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(m_highScorekey, score);
    }

    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(m_highScorekey))
        {
            return PlayerPrefs.GetInt(m_highScorekey);
        }
        else
        {
            return 0;
        }
    }
}
