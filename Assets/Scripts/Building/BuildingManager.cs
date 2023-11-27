using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingManager : MonoBehaviour
{
    [Header("Building settings")]
    [SerializeField]
    private GameObject buildingPrefab;
    [SerializeField]
    private BaseGenerator baseGenerator;
    [SerializeField]
    private CustomCursor customCursor;
    [Header("Repair settings")]
    [SerializeField]
    private Sprite fixingSprite;
    [SerializeField]
    private int fixMoney;
    [SerializeField]
    private int fixStone;
    [SerializeField]
    private int fixWood;
    [Header("Repair UI")]
    [SerializeField]
    private TMP_Text moneyVal;
    [SerializeField]
    private TMP_Text stoneVal;
    [SerializeField]
    private TMP_Text woodVal;

    private ResourceManager resourceManager;

    private HashSet<Vector2Int> buildingsPositions = new HashSet<Vector2Int>();
    private Building currentBuilding;
    private GameObject currentTower = null;
    private bool fixing = false;

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        baseGenerator = FindObjectOfType<BaseGenerator>();
        moneyVal.text = fixMoney.ToString();
        stoneVal.text = fixStone.ToString();
        woodVal.text = fixWood.ToString();
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
                foreach (var tile in baseGenerator.wallPositions.Keys)
                {
                    float dist = Vector2.Distance(baseGenerator.CellToWorld((Vector3Int)tile) + new Vector3(0, 1, 0), Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (dist < nearestDistance)
                    {
                        nearestTile = tile;
                        nearestDistance = dist;
                    }
                }
                if (nearestDistance < 0.5f)
                {
                    var w = baseGenerator.wallPositions[nearestTile].GetComponent<Wall>();
                    if (!w.destroyed && w.tower == null)
                    {
                        Build(currentTower, nearestTile);
                    }
                }
            }
            else if (fixing)
            {
                Vector2Int nearestTile = Vector2Int.zero;
                float nearestDistance = float.MaxValue;
                foreach (var tile in baseGenerator.wallPositions.Keys)
                {
                    float dist = Vector2.Distance(baseGenerator.CellToWorld((Vector3Int)tile), Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (dist < nearestDistance)
                    {
                        nearestTile = tile;
                        nearestDistance = dist;
                    }
                }
                if (nearestDistance < 0.5f)
                {
                    var w = baseGenerator.wallPositions[nearestTile].GetComponent<Wall>();
                    if (w.destroyed)
                    {
                        w.Fix();
                        DecreaseResources(fixMoney, fixStone, fixWood);
                        Debug.Log("Wall fixed");
                        ResetState();
                    }
                }
            }
        }
    }

    public void TryToBuild(Building building)
    {
        if (CheckCost(building.money, building.stone, building.wood))
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = building.sprite;
            currentBuilding = building;
        }
    }

    public void TryToBuild(GameObject tower)
    {
        Tower t = tower.GetComponentInChildren<Tower>();
        if (CheckCost(t.money, t.stone, t.wood))
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = tower.GetComponent<SpriteRenderer>().sprite;
            currentTower = tower;
        }
    }

    public void TryToFix()
    {
        currentBuilding = null;
        currentTower = null;
        if (CheckCost(fixMoney, fixStone, fixWood))
        {
            fixing = !fixing;
            if (fixing)
            {
                customCursor.gameObject.SetActive(true);
                customCursor.GetComponent<SpriteRenderer>().sprite = fixingSprite;
            }
            else
            {
                ResetState();
            }
        }
    }

    private void Build(Building building, Vector2Int position)
    {
        buildingsPositions.Add(position);
        buildingPrefab.GetComponent<BuildingFunctionality>().building = currentBuilding;
        baseGenerator.CreateBuilding(buildingPrefab, position);
        DecreaseResources(building.money, building.stone, building.wood);
        ResetState();
    }

    private void Build(GameObject tower, Vector2Int position)
    {
        var t = baseGenerator.CreateBuilding(tower, position);
        var w = baseGenerator.wallPositions[position];
        t.transform.SetParent(w.transform);
        w.GetComponent<Wall>().AttachTower(t);
        //t.GetComponent<SpriteRenderer>().sortingOrder = 1; // TODO: change logic of tower sorting
        var tow = tower.GetComponentInChildren<Tower>();
        DecreaseResources(tow.money, tow.stone, tow.wood);
        ResetState();
    }

    public void ResetState()
    {
        customCursor.gameObject.SetActive(false);
        currentBuilding = null;
        currentTower = null;
        fixing = false;
    }

    private bool CheckCost(int money, int stone, int wood)
    {
        return 
            resourceManager.GetResourceAmount("money") >= money &&
            resourceManager.GetResourceAmount("stone") >= stone &&
            resourceManager.GetResourceAmount("wood") >= wood;
    }

    private void DecreaseResources(int money, int stone, int wood)
    {
        resourceManager.DecreaseResources("money", money);
        resourceManager.DecreaseResources("stone", stone);
        resourceManager.DecreaseResources("wood", wood);
    }
}
