using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum Direction
{
    NORTH = 0,
    EAST = 1,
    SOUTH = 2,
    WEST = 3,
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Vector3Int startPosition;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject[] enemyList;

    public Button victoryReplayButton;
    public GameObject victoryUI;

    public List<Vector3> waypoints = new List<Vector3>();

    private WaveManager waveManager;

    private float countdown = 2f;
    [SerializeField] private float waveCountdown = 5f;

    private void Start()
    {
        WaypointManager.tileMapManager = GameObject.Find(ObjectName.gameManager).GetComponent<TileMapManager>();
        WaypointManager.tilemap = this.tilemap;

        if (GameObject.Find(ObjectName.optionManager).GetComponent<OptionManager>().shouldGenerateMap)
        {
            this.startPosition = WaypointManager.resetMap(24, 16);
        }
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
        //Check if the game should end, only end if there's no enemy left
        if (waveManager.waveNumber > waveManager.maxWave && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
                Time.timeScale = 0;
                GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>().SetGameIsPaused(true);
                victoryUI.SetActive(true);                
        }
    }


    public class WaveManager
    {
        List<Vector3> waypoints;
        Vector3 startPosition;

        public GameObject UI;

        private DataManager dataManager;

        public Button victoryReplayButton;
        public GameObject victoryUI;

        public int waveNumber = 1;
        public float timeBetweenEnemy = 0.5f;
        public int maxWave = 72;

        public GameObject[] enemyList;
        public WaveManager(List<Vector3> waypoints, Vector3 startPosition, GameObject[] enemyList)
        {
            this.waypoints = waypoints;
            this.startPosition = startPosition;
            dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();
            this.enemyList = enemyList;
        }

        private void SpawnEnemy()
        {
            int randomNumber = Random.Range(0, enemyList.Length);
            Enemy enemy = Instantiate(enemyList[randomNumber], startPosition, Quaternion.identity).GetComponent<Enemy>();
            enemy.life += (int) waveNumber / 5f;
            enemy.Init(waypoints);
        }

        public IEnumerator SpawnWave()
        {
            //Only spawn wave if we're not at the end;
            if(waveNumber <= maxWave)
            {
                dataManager.ChangeWave(waveNumber);
                for (int i = 0; i < waveNumber; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(timeBetweenEnemy);
                }

                if (timeBetweenEnemy > 0)
                {
                    timeBetweenEnemy -= 0.01f;
                }
                waveNumber++;
            } 
        }
    }

    public static class WaypointManager
    {
        public static List<Vector3> waypoints = new List<Vector3>();
        public static Tilemap tilemap;
        public static TileMapManager tileMapManager;

        public static Vector3Int resetMap(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = -1; j < height; j++)
                {
                    Vector3Int currentPos = new Vector3Int(i, j, 0);
                    tilemap.SetTile(currentPos, tileMapManager.tileList[(int)TileType.GRASS]);
                }
            }

            tilemap.SetTile(new Vector3Int(23, 10, 0), tileMapManager.tileList[(int)TileType.PATH]);

            return generatePath();
        }

        public static Vector3Int generatePath()
        {
            Vector3Int castlePos = new Vector3Int(23, 10, 0);

            return generatePathRec(300, castlePos, Direction.WEST);
        }

        private static Vector3Int generatePathRec(int pathSize, Vector3Int currentPos, Direction direction)
        {
            if (pathSize == 0)
            {
                return currentPos;
            }

            var randNextDirection = (int)Random.Range(0, 6);
            var possibleNextPosition = getNextDirection(currentPos, randNextDirection, direction);

            int i = 0;
            while (!canBePath(possibleNextPosition, currentPos) && i < 5)
            {
                possibleNextPosition = getNextDirection(currentPos, i, direction);
                i++;
            }

            if (i == 5)
            {
                return generatePathRec(0, currentPos, getCurrentDirection(possibleNextPosition, currentPos)); ;
            }

            tilemap.SetTile(possibleNextPosition, tileMapManager.tileList[(int)TileType.PATH]);

            if (pathSize - 1 == 0)
            {
                return possibleNextPosition;
            }
            return generatePathRec(pathSize - 1, possibleNextPosition, getCurrentDirection(possibleNextPosition, currentPos));
        }

        private static Direction getCurrentDirection(Vector3Int newPos, Vector3Int prevPos)
        {
            if (newPos.x - prevPos.x == -1)
            {
                return Direction.WEST;
            }

            if (newPos.x - prevPos.x == 1)
            {
                return Direction.EAST;
            }

            if (newPos.y - prevPos.y == 1)
            {
                return Direction.NORTH;
            }

            return Direction.SOUTH;

        }

        private static Vector3Int getNextDirection(Vector3Int currentPos, int nextDirection, Direction direction)
        {
            switch(nextDirection)
            {
                case 0:
                    return getVerticalPos(currentPos, true);
                case 1:
                    return getVerticalPos(currentPos, false);
                case 2:
                    return getHorizontalPos(currentPos, true);
                case 3:
                    return getHorizontalPos(currentPos, false);
                default:
                    switch(direction)
                    {
                        case Direction.WEST:
                            return new Vector3Int(currentPos.x - 1, currentPos.y, 0);
                        case Direction.EAST:
                            return new Vector3Int(currentPos.x + 1, currentPos.y, 0);
                        case Direction.NORTH:
                            return new Vector3Int(currentPos.x, currentPos.y + 1, 0);
                        default:
                            return new Vector3Int(currentPos.x, currentPos.y - 1, 0);
                    }
            }
        }

        private static Vector3Int getVerticalPos(Vector3Int pos, bool opt)
        {
            int leftPathCount = 0;
            int rightPathCount = 0;

            for (int i = 0; i < pos.x; i++)
            {
                Vector3Int newPos = new Vector3Int(i, pos.y, 0);
                if (tilemap.GetTile(newPos).name == tileMapManager.tileName[(int)TileType.PATH])
                {
                    leftPathCount += 1;
                }
            }

            for (int i = pos.x + 1; i < 24; i++)
            {
                Vector3Int newPos = new Vector3Int(i, pos.y, 0);
                if (tilemap.GetTile(newPos).name == tileMapManager.tileName[(int)TileType.PATH])
                {
                    rightPathCount += 1;
                }
            }

            if (leftPathCount < rightPathCount && canBePath(new Vector3Int(pos.x - 1, pos.y, 0), pos))
            {
                return new Vector3Int(pos.x - 1, pos.y, 0);
            }
            else if (leftPathCount > rightPathCount && canBePath(new Vector3Int(pos.x + 1, pos.y, 0), pos))
            {
                return new Vector3Int(pos.x + 1, pos.y, 0);
            }

            if (pos.x > 12 && canBePath(new Vector3Int(pos.x - 1, pos.y, 0), pos))
            {
                return new Vector3Int(pos.x - 1, pos.y, 0);
            } else if (canBePath(new Vector3Int(pos.x + 1, pos.y, 0), pos))
            {
                return new Vector3Int(pos.x + 1, pos.y, 0);
            }

            if (opt)
            {
                return new Vector3Int(pos.x - 1, pos.y, 0);
            }

            return new Vector3Int(pos.x + 1, pos.y, 0);
        }


        private static Vector3Int getHorizontalPos(Vector3Int pos, bool opt)
        {
            int botPathCount = 0;
            int topPathCount = 0;

            for (int i = -1; i < pos.y; i++)
            {
                Vector3Int newPos = new Vector3Int(pos.x, i, 0);
                if (tilemap.GetTile(newPos).name == tileMapManager.tileName[(int)TileType.PATH])
                {
                    botPathCount += 1;
                }
            }

            for (int i = pos.y + 1; i < 16; i++)
            {
                Vector3Int newPos = new Vector3Int(pos.x, i, 0);
                if (tilemap.GetTile(newPos).name == tileMapManager.tileName[(int)TileType.PATH])
                {
                    topPathCount += 1;
                }
            }

            if (botPathCount < topPathCount && canBePath(new Vector3Int(pos.x, pos.y - 1, 0), pos))
            {
                return new Vector3Int(pos.x, pos.y - 1, 0);
            } else if (botPathCount > topPathCount && canBePath(new Vector3Int(pos.x, pos.y + 1, 0), pos))
            {
                return new Vector3Int(pos.x, pos.y + 1, 0);
            }

            if (pos.y > 8 && canBePath(new Vector3Int(pos.x, pos.y - 1, 0), pos))
            {
                return new Vector3Int(pos.x, pos.y - 1, 0);
            }
            else if (canBePath(new Vector3Int(pos.x, pos.y + 1, 0), pos))
            {
                return new Vector3Int(pos.x, pos.y + 1, 0);
            }

            if (opt)
            {
                return new Vector3Int(pos.x, pos.y - 1, 0);
            }

            return new Vector3Int(pos.x, pos.y + 1, 0);
        }
        private static bool canBePath(Vector3Int pos, Vector3Int lastPos)
        {
            if (tilemap.GetTile(pos).name != tileMapManager.tileName[(int)TileType.GRASS])
            {
                return false;
            }

            Vector3Int verifyPos = new Vector3Int(pos.x + 1, pos.y, 0);
            if (verifyPos != lastPos && !isGrassOrBorder(verifyPos)) {
                return false;
            }

            verifyPos.x -= 2;
            if (verifyPos != lastPos && !isGrassOrBorder(verifyPos))
            {
                return false;
            }

            verifyPos.x += 1;
            verifyPos.y += 1;
            if (verifyPos != lastPos && !isGrassOrBorder(verifyPos))
            {
                return false;
            }

            verifyPos.y -= 2;
            if (verifyPos != lastPos && !isGrassOrBorder(verifyPos))
            {
                return false;
            }

            return true;
        }

        private static bool isGrassOrBorder(Vector3Int pos)
        {
            return tilemap.GetTile(pos).name == tileMapManager.tileName[(int)TileType.GRASS] 
                    || tilemap.GetTile(pos).name == tileMapManager.tileName[(int)TileType.BORDER];
        }

        public static List<Vector3> generateWaypoint(Vector3Int startPos, Tilemap tilemap)
        {
            waypoints.Clear();

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
