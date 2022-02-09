using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    private List<Vector3> waypoints;
    [SerializeField] private Tilemap tileMap;

    [SerializeField] private float speed = 2f;
    private int index = 0;

    private void Start()
    {
        // Get waypoint to follow
        waypoints = GameObject.Find("EnemyWaypointManager").GetComponent<EnemyWaypointManager>().waypoints;

        // Teleport enemy to first waypoint
        transform.position = waypoints[index];
    }

    private void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        // loop until last waypoint is reached  
        if (index < waypoints.Count)
        {
            // Move Enemy from current waypoint to the next one
            transform.position = Vector2.MoveTowards(transform.position,
               new Vector2(waypoints[index].x, waypoints[index].y),
               speed * Time.deltaTime);

            // if reached next waypoint
            if (transform.position == waypoints[index])
            {
                index++;
            }
        }
    }

}