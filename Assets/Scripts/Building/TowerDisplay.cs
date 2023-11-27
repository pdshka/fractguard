using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject tower;
    [SerializeField]
    private TMP_Text moneyVal;
    [SerializeField]
    private TMP_Text stoneVal;
    [SerializeField]
    private TMP_Text woodVal;

    private void Start()
    {
        Tower t = tower.GetComponentInChildren<Tower>();
        moneyVal.text = t.money.ToString();
        stoneVal.text = t.stone.ToString();
        woodVal.text = t.wood.ToString();
    }
}
