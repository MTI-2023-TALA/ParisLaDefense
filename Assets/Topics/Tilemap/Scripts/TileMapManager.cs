using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private void Start()
    {
        dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();
    }

    public void Update()
    {
        // Handle click event 
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3Int mousePos = getMousePosition();
            TileBase tileBase = getTileBase(mousePos);
            if (tileBase.name == tileName[(int)TileType.GRASS])
            {
                SpawnTurret(mousePos);
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

    private void SpawnTurret(Vector3Int mousePos)
    {
        if (dataManager.RemoveGold(turret.GetComponent<Turret>().cost))
        {
            tileMap.SetTile(mousePos, tileList[(int)TileType.TOWER]);
            Instantiate(turret, new Vector2(mousePos.x + 0.5f, mousePos.y + 0.5f), Quaternion.identity);
        }
    }
}
