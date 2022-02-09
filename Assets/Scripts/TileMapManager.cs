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

    public void Update()
    {
        // Handle click event 
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3Int mousePos = getMousePosition();
            TileBase tileBase = getTileBase(mousePos);
            if (tileBase.name == tileName[(int)TileType.GRASS])
            {
                tileMap.SetTile(mousePos, tileList[(int)TileType.TOWER]);
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

}
