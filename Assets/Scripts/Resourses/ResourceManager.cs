using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text moneyAmount;
    [SerializeField]
    private TMP_Text woodAmount;
    [SerializeField]
    private TMP_Text stoneAmount;

    private Dictionary<string, int> resources = new Dictionary<string, int>();

    private void Start()
    {
        resources["money"] = 0;
        resources["wood"] = 0;
        resources["stone"] = 0;
    }

    private void Update()
    {
        moneyAmount.text = resources["money"].ToString();
        woodAmount.text = resources["wood"].ToString();
        stoneAmount.text = resources["stone"].ToString();
    }

    public void IncreaseResources(string resourceName, int amount)
    {
        resources[resourceName] += amount;
    }
}
