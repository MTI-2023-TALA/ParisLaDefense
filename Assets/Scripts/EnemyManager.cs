using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Vector3Int startPosition;
    [SerializeField] private Tilemap tileMap;

    // Expose waypoint to the enemy Unit
    public List<Vector3> waypoints = new List<Vector3>();

    [SerializeField] GameObject[] enemyList;

    private void Start()
    {
        calculTeWaypoints();
        SpawnWave(0);
    }

    private void calculTeWaypoints()
    {
        addWaypoint(startPosition);
        calcultateNextPosition(startPosition, startPosition, false);
        Debug.Log("Waypoint Calculated !");
    }

    private void calcultateNextPosition(Vector3Int previousPos, Vector3Int currentPos, bool checkPrevious = true)
    {
        // We check the 4 tile around the enemy and check if we can forward to the next path

        Vector3Int potiantialNextPostition = currentPos;

        potiantialNextPostition.x += 1;
        if (isNextPositionValid(previousPos, potiantialNextPostition, checkPrevious))
        {
            addWaypoint(potiantialNextPostition);
            calcultateNextPosition(currentPos, potiantialNextPostition);
            return;
        }

        potiantialNextPostition.x -= 2;
        if (isNextPositionValid(previousPos, potiantialNextPostition, checkPrevious))
        {
            addWaypoint(potiantialNextPostition);
            calcultateNextPosition(currentPos, potiantialNextPostition);
            return;
        }

        potiantialNextPostition.x += 1;
        potiantialNextPostition.y += 1;
        if (isNextPositionValid(previousPos, potiantialNextPostition, checkPrevious))
        {
            addWaypoint(potiantialNextPostition);
            calcultateNextPosition(currentPos, potiantialNextPostition);
            return;
        }

        potiantialNextPostition.y -= 2;
        if (isNextPositionValid(previousPos, potiantialNextPostition, checkPrevious))
        {
            addWaypoint(potiantialNextPostition);
            calcultateNextPosition(currentPos, potiantialNextPostition);
            return;
        }

        return;
    }

    private bool isNextPositionValid(Vector3Int previousPos, Vector3Int nextPos, bool checkPreviousPos = true)
    {
        return (!checkPreviousPos || previousPos != nextPos) && tileMap.GetTile(nextPos).name == "path";
    }

    private void addWaypoint(Vector3Int pos)
    {
        waypoints.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z));
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemyList[0], startPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.Init(waypoints);
    }
    
    public void SpawnWave(int wave)
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnEnemy();
        }
    }
}
