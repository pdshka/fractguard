using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [Header("Initial resources")]
    [SerializeField] private int initialMoney = 300;
    [SerializeField] private int initialStone = 200;
    [SerializeField] private int initialWood = 200;
    [Header("UI text fields")]
    [SerializeField]
    private TMP_Text moneyAmount;
    [SerializeField]
    private TMP_Text woodAmount;
    [SerializeField]
    private TMP_Text stoneAmount;

    private Dictionary<string, int> resources = new Dictionary<string, int>();

    private void Start()
    {
        resources["money"] = initialMoney;
        resources["stone"] = initialStone;
        resources["wood"] = initialWood;
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

    public void DecreaseResources(string resourceName, int amount)
    {
        resources[resourceName] -= amount;
    }

    public int GetResourceAmount(string resourceName)
    {
        return resources[resourceName];
    }
}
