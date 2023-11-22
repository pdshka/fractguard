using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("Building settings")]
    [SerializeField]
    private GameObject buildingPrefab;
    [SerializeField]
    private BaseGenerator baseGenerator;
    [SerializeField]
    private CustomCursor customCursor;
    private HashSet<Vector2Int> buildingsPositions;
    private Building currentBuilding;
    private ResourceManager resourceManager;

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        baseGenerator = FindObjectOfType<BaseGenerator>();
    }

    public void TryToBuild(Building building)
    {
        if (resourceManager.GetResourceAmount("money") >= building.cost)
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = building.sprite;
            currentBuilding = building;
        }
    }

    public void SetDefaultValues()
    {
        customCursor.gameObject.SetActive(false);
        currentBuilding = null;
    }
}
