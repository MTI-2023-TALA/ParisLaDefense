using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// If you update enum dont forget to update tileList and tileName
public enum TileType: int
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

    public GameObject turret;

    private DataManager dataManager;
    private TowerUIManager towerUIManager;

    public Button btnSpawnTurret;
    private Vector3Int lastClickPosition;

    private void Start()
    {
        dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();
        towerUIManager = GameObject.Find(ObjectName.gameManager).GetComponent<TowerUIManager>();
        towerUIManager.removeTowerUI();
        btnSpawnTurret.onClick.AddListener(SpawnTurret);
    }

    private void Update()
    {
        // Handle click event 
        if (Input.GetButtonDown("Fire1"))
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
                towerUIManager.moveTowerUi(new Vector3(mousePos.x + 0.5f , mousePos.y + 0.5f, mousePos.z));
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

    public void SpawnTurret()
    {
        if (dataManager.RemoveGold(turret.GetComponent<Turret>().cost))
        {
            tileMap.SetTile(this.lastClickPosition, tileList[(int)TileType.TOWER]);
            Instantiate(turret, new Vector2(this.lastClickPosition.x + 0.5f, this.lastClickPosition.y + 0.5f), Quaternion.identity);
            towerUIManager.removeTowerUI();
        }
    }
}
