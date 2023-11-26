using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingDisplay : MonoBehaviour
{
    [SerializeField]
    private Building building;
    [SerializeField]
    private TMP_Text moneyVal;
    [SerializeField]
    private TMP_Text stoneVal;
    [SerializeField]
    private TMP_Text woodVal;


    private void Start()
    {
        GetComponent<Image>().sprite = building.sprite;
        moneyVal.text = building.money.ToString();
        stoneVal.text = building.stone.ToString();
        woodVal.text = building.wood.ToString();
    }
}
