using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    private List<Vector3> waypoints;
    [SerializeField] private Tilemap tileMap;

    [SerializeField] private float speed = 2f;
    public float life = 2f;
    public int gold = 10;

    private DataManager dataManager;

    private int index = 0;

    public void Init(List<Vector3> waypoints)
    {
        // Get waypoint to follow
        this.waypoints = waypoints;
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();


        // Teleport enemy to first waypoint
        transform.position = waypoints[index];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Castle")
        {
            return;
        }

        dataManager.TakeDamage();

        Destroy(gameObject);
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

    public void TakeDamage(float damage)
    {
        life -= damage;
        if(life <= 0f)
        {
            dataManager.AddGold(gold);
            Destroy(gameObject);
        }
    }
}