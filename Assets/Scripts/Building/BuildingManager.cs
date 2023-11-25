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
    private HashSet<Vector2Int> towersPositions = new HashSet<Vector2Int>();
    private GameObject currentTower = null;

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        baseGenerator = FindObjectOfType<BaseGenerator>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (currentBuilding != null)
            {
                Vector2Int nearestTile = Vector2Int.zero;
                float nearestDistance = float.MaxValue;
                foreach (var tile in baseGenerator.floorPositions)
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
            else if (currentTower != null)
            {
                Vector2Int nearestTile = Vector2Int.zero;
                float nearestDistance = float.MaxValue;
                foreach (var tile in baseGenerator.wallPositions)
                {
                    float dist = Vector2.Distance(baseGenerator.CellToWorld((Vector3Int)tile) + new Vector3(0, 1, 0), Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (dist < nearestDistance)
                    {
                        nearestTile = tile;
                        nearestDistance = dist;
                    }
                }
                if (!towersPositions.Contains(nearestTile) && nearestDistance < 0.5f)
                {
                    Build(currentTower, nearestTile);
                }
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

    public void TryToBuild(GameObject tower)
    {
        Tower t = tower.GetComponent<Tower>();
        if (resourceManager.GetResourceAmount("money") >= t.cost)
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = t.GetComponent<SpriteRenderer>().sprite;
            currentTower = tower;
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

    private void Build(GameObject tower, Vector2Int position)
    {
        towersPositions.Add(position);
        var t = baseGenerator.CreateBuilding(tower, position);
        t.GetComponent<SpriteRenderer>().sortingOrder = 1; // TODO: change logic of tower sorting
        resourceManager.DecreaseResources("money", tower.GetComponent<Tower>().cost);
        ResetState();
    }

    public void ResetState()
    {
        customCursor.gameObject.SetActive(false);
        currentBuilding = null;
        currentTower = null;
    }
}
