using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_highScore;
    private string m_newGameScene = "Level1";

    // Start is called before the first frame update
    void Start()
    {
        // Set the high score text
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        m_highScore.text = $"Most Waves Survived: {highScore}";

        // Enable the cursor and unlock it so you can interact with the UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartNewGame()
    {
        // Disable the cursor and lock it so it doesn't interfere with the game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(m_newGameScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
