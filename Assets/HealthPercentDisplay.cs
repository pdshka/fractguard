using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthPercentDisplay : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private TMP_Text percent;
    [SerializeField] private string sceneName;

    private void Update()
    {
        percent.text = ((int)((float)health.currentHealth / health.GetInitialHealth() * 100)).ToString() + "%";
        if (health.currentHealth <= 0)
            EndGame();
    }

    private void EndGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
