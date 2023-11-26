using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    [SerializeField] private Image background;
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
    }
}
