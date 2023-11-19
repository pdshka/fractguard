using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string sceneName;
    public GameObject settingsPanel;
    
    public void NewGamePressed()
    {
        Debug.Log("New game pressed.");
        SceneManager.LoadScene(sceneName);
    }

    public void ContinuePressed()
    {
        Debug.Log("Continue pressed.");
        SceneManager.LoadScene(sceneName);
    }

    public void SettingsPressed()
    {
        Debug.Log("Settings pressed.");
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitPressed()
    {
        Debug.Log("Exit pressed.");
        Application.Quit();
    }
}
