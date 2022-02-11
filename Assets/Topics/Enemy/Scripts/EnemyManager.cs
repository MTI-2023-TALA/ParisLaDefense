using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Vector3Int startPosition;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject[] enemyList;

    private List<Vector3> waypoints = new List<Vector3>();

    private WaveManager waveManager;

    private float countdown = 2f;
    [SerializeField] private float waveCountdown = 5f;

    private void Start()
    {
        waypoints = WaypointManager.generateWaypoint(startPosition, tilemap);

        waveManager = new WaveManager(waypoints, startPosition, enemyList);
    }

    private void Update()
    {
        if(countdown <= 0f)
        {
            StartCoroutine(waveManager.SpawnWave());
            countdown = waveCountdown + waveManager.timeBetweenEnemy * waveManager.waveNumber;
        }
        countdown -= Time.deltaTime;
    }


    public class WaveManager
    {
        List<Vector3> waypoints;
        Vector3 startPosition;

        public GameObject UI;

        private DataManager dataManager;

        public int waveNumber = 1;
        public float timeBetweenEnemy = 0.5f;

        GameObject[] enemyList;
        public WaveManager(List<Vector3> waypoints, Vector3 startPosition, GameObject[] enemyList)
        {
            this.waypoints = waypoints;
            this.startPosition = startPosition;
            dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();
            this.enemyList = enemyList;
        }

        private void SpawnEnemy()
        {
            Enemy enemy = Instantiate(enemyList[0], startPosition, Quaternion.identity).GetComponent<Enemy>();
            enemy.Init(waypoints);
        }

        public IEnumerator SpawnWave()
        {
            dataManager.ChangeWave(waveNumber);
            for (int i = 0; i < waveNumber; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(timeBetweenEnemy);
            }
            waveNumber++;
            
        }
    }

    public static class WaypointManager
    {
        public static List<Vector3> waypoints = new List<Vector3>();
        private static Tilemap tilemap;

        public static List<Vector3> generateWaypoint(Vector3Int startPos, Tilemap tilemap)
        {
            WaypointManager.tilemap = tilemap;
            CalculateWaypoint(startPos);
            return waypoints;
        }

        private static void CalculateWaypoint(Vector3Int startPos)
        {
            AddWaypoint(startPos);
            CalcultateNextPosition(startPos, startPos, false);
            Debug.Log("Waypoint Calculated !");
        }

        private static void CalcultateNextPosition(Vector3Int previousPos, Vector3Int currentPos, bool checkPrevious = true)
        {
            // We check the 4 tile around the enemy and check if we can forward to the next path

            Vector3Int nextPos = currentPos;

            nextPos.x += 1;
            if (IsNextPositionValid(previousPos, nextPos, checkPrevious))
            {
                AddWaypoint(nextPos);
                CalcultateNextPosition(currentPos, nextPos);
                return;
            }

            nextPos.x -= 2;
            if (IsNextPositionValid(previousPos, nextPos, checkPrevious))
            {
                AddWaypoint(nextPos);
                CalcultateNextPosition(currentPos, nextPos);
                return;
            }

            nextPos.x += 1;
            nextPos.y += 1;
            if (IsNextPositionValid(previousPos, nextPos, checkPrevious))
            {
                AddWaypoint(nextPos);
                CalcultateNextPosition(currentPos, nextPos);
                return;
            }

            nextPos.y -= 2;
            if (IsNextPositionValid(previousPos, nextPos, checkPrevious))
            {
                AddWaypoint(nextPos);
                CalcultateNextPosition(currentPos, nextPos);
                return;
            }

            return;
        }

        private static bool IsNextPositionValid(Vector3Int previousPos, Vector3Int nextPos, bool checkPreviousPos = true)
        {
            return (!checkPreviousPos || previousPos != nextPos) && WaypointManager.tilemap.GetTile(nextPos).name == TileName.path;
        }

        private static void AddWaypoint(Vector3Int pos)
        {
            waypoints.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z));
        }
    }
}
