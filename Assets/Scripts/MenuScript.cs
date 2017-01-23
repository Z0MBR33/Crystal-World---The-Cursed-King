using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void StartGame()
    {
        GameStats.LoadLevelSettings = true;
        GameStats.Level = 1;

        GameStats.LoadCharStats = false;

        ChangeScene("Scenes/World");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}