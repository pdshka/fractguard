using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingDisplay : MonoBehaviour
{
    [SerializeField]
    private Building building;

    private void Start()
    {
        GetComponent<Image>().sprite = building.sprite;
        GetComponentInChildren<TMP_Text>().text = building.cost.ToString();
    }
}
