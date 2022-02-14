using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// If you update enum dont forget to update tileList and tileName
public enum TileType : int
{
    GRASS = 0,
    PATH = 1,
    BORDER = 2,
    TOWER = 3
}
public class TileMapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile[] tileList;
    [SerializeField] private string[] tileName;

    public GameObject canoneer;
    public GameObject oilThrower;

    private DataManager dataManager;
    private TowerUIManager towerUIManager;

    public Button btnSpawnCanoneer;
    public Button btnSpawnOilThrower;
    public Button btnSellTurret;
    public Button btnLevelUp;
    private Vector3Int lastClickPosition;

    private void Start()
    {
        dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();
        towerUIManager = GameObject.Find(ObjectName.gameManager).GetComponent<TowerUIManager>();
        towerUIManager.removeTowerUI();
        btnSpawnCanoneer.onClick.AddListener(SpawnCanoneer);
        btnSpawnOilThrower.onClick.AddListener(SpawnOilThrower);
        btnSellTurret.onClick.AddListener(delegate { SellTurret(GetClosestTurret()); });
        btnLevelUp.onClick.AddListener(delegate { UpgradeTurret(GetClosestTurret()); });
    }

    private void Update()
    {
        // Handle click event
        if (Input.GetButtonDown("Fire1") && !dataManager.GetGameIsPaused())
        {
            if (towerUIManager.isTowerUIActive())
            {
                return;
            }

            Vector3Int mousePos = getMousePosition();
            TileBase tileBase = getTileBase(mousePos);
            this.lastClickPosition = mousePos;
            if (tileBase.name == tileName[(int)TileType.GRASS])
            {
                towerUIManager.moveTowerUi(new Vector3(mousePos.x + 0.5f, mousePos.y + 0.5f, mousePos.z));
            }
            if (tileBase.name == tileName[(int)TileType.TOWER])
            {
                towerUIManager.moveTowerUiUpgrade(new Vector3(mousePos.x + 0.5f, mousePos.y + 0.5f, mousePos.z));
            }
        }
    }

    private TileBase getTileBase(Vector3Int pos)
    {
        return tileMap.GetTile(pos);
    }

    private Vector3Int getMousePosition()
    {
        Camera camera = Camera.main;
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);

        return Vector3Int.FloorToInt(worldPos);
    }

    public void SpawnCanoneer()
    {
        if (dataManager.RemoveGold(canoneer.GetComponent<Turret>().cost))
        {
            tileMap.SetTile(this.lastClickPosition, tileList[(int)TileType.TOWER]);
            Instantiate(canoneer, new Vector2(this.lastClickPosition.x + 0.5f, this.lastClickPosition.y + 0.5f), Quaternion.identity);
            towerUIManager.removeTowerUI();
        }
    }

    public void SpawnOilThrower()
    {
        if (dataManager.RemoveGold(oilThrower.GetComponent<Turret>().cost))
        {
            tileMap.SetTile(this.lastClickPosition, tileList[(int)TileType.TOWER]);
            Instantiate(oilThrower, new Vector2(this.lastClickPosition.x + 0.5f, this.lastClickPosition.y + 0.5f), Quaternion.identity);
            towerUIManager.removeTowerUI();
        }
    }

    public void SellTurret(GameObject closestTurret)
    {
        if (closestTurret != null)
        {
            Turret turret = closestTurret.GetComponent<Turret>();
            int money = turret.CalculateSell();
            Destroy(closestTurret.gameObject);
            dataManager.AddGold(money);
            tileMap.SetTile(this.lastClickPosition, tileList[(int)TileType.GRASS]);
        }
        towerUIManager.removeTowerUI();
    }

    public void UpgradeTurret(GameObject closestTurret)
    {
        if (closestTurret != null)
        {
            Debug.Log("ici");
            Turret turret = closestTurret.GetComponent<Turret>();
            int money = turret.CalculateUpgrade();
            if (dataManager.RemoveGold(money))
            {
                turret.LevelUp(1);
                GameObject.Find(ObjectName.gameManager).GetComponent<TowerUIManager>().updateTowerUpgradeCost(turret);
            }

        }
    }

    public GameObject GetClosestTurret()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(Tag.turret);
        float shortestDist = Mathf.Infinity;
        GameObject closestTurret = null;
        Vector2 mousePos = new Vector2(lastClickPosition.x + 0.5f, lastClickPosition.y + 0.5f);
        foreach (GameObject turret in turrets)
        {
            Debug.Log("mousePos: " + mousePos + ", turretPos: " + turret.transform.position);
            float curDist = Vector2.Distance(mousePos, turret.transform.position);
            if (curDist < shortestDist)
            {
                shortestDist = curDist;
                closestTurret = turret;
            }
        }
        return closestTurret;
    }
}
