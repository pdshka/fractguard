using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Sprite sprite;

    private int possibility = 5;

    private void Start()
    {
        int r = Random.Range(0, 100);
        if (0 <= r && r < possibility)
        {
            background.color = Color.white;
            background.sprite = sprite;
        }
        StartCoroutine(LightenBlackScreen());
    }

    private IEnumerator LightenBlackScreen()
    {
        while (blackScreen.color.a > 0)
        {
            yield return new WaitForSeconds(0.1f);
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - 0.05f);
        }
        blackScreen.gameObject.SetActive(false);
    }
}
