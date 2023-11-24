using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFunctionality : MonoBehaviour
{
    public Building building;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private ResourceManager resourceManager;

    private float nextIncreaseTime;

    private void Start()
    {
        spriteRenderer.sprite = building.sprite;
        resourceManager = FindObjectOfType<ResourceManager>();
        nextIncreaseTime = Time.time + building.timeBetweenIncreases;
    }

    private void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            resourceManager.IncreaseResources(building.resource.resourceName, building.resourceIncrease);
            nextIncreaseTime = Time.time + building.timeBetweenIncreases;
        }
    }
}
