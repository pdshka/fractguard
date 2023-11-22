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

    private ResourceManager resourceManager;

    private HashSet<Vector2Int> buildingsPositions = new HashSet<Vector2Int>();
    private Building currentBuilding;

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        baseGenerator = FindObjectOfType<BaseGenerator>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && currentBuilding != null)
        {
            Vector2Int nearestTile = Vector2Int.zero;
            float nearestDistance = float.MaxValue;
            foreach(var tile in baseGenerator.floorPositions)
            {
                float dist = Vector2.Distance(baseGenerator.CellToWorld((Vector3Int)tile), Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < nearestDistance)
                {
                    nearestTile = tile;
                    nearestDistance = dist;
                }
            }
            if (!buildingsPositions.Contains(nearestTile) && nearestDistance < 0.5f)
            {
                Build(currentBuilding, nearestTile);
            }
        }
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

    private void Build(Building building, Vector2Int position)
    {
        buildingsPositions.Add(position);
        buildingPrefab.GetComponent<BuildingFunctionality>().building = currentBuilding;
        baseGenerator.CreateBuilding(buildingPrefab, position);
        resourceManager.DecreaseResources("money", currentBuilding.cost);
        ResetState();
    }

    public void ResetState()
    {
        customCursor.gameObject.SetActive(false);
        currentBuilding = null;
    }
}
