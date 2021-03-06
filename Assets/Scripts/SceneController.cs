using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
