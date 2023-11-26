using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDisplay : MonoBehaviour
{
    [SerializeField]
    private Tower tower;
    [SerializeField]
    private TMP_Text moneyVal;
    [SerializeField]
    private TMP_Text stoneVal;
    [SerializeField]
    private TMP_Text woodVal;

    private void Start()
    {
        moneyVal.text = tower.money.ToString();
        stoneVal.text = tower.stone.ToString();
        woodVal.text = tower.wood.ToString();
    }
}
