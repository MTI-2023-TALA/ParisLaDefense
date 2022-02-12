using System.Collections;
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
    private bool slowed = false;

    private DataManager dataManager;
    public SpriteRenderer spriteRenderer;

    private int index = 0;

    public void Init(List<Vector3> waypoints)
    {
        // Get waypoint to follow
        this.waypoints = waypoints;
        dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();


        // Teleport enemy to first waypoint
        transform.position = waypoints[index];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tag.castle)
        {
            dataManager.TakeDamage();
            Destroy(gameObject);
        }

        return;

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

            // flip left to face next waypoint
            if (transform.position.x > waypoints[index].x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            // flip right
            else
                transform.rotation = Quaternion.Euler(0, 180f, 0);

            // if reached next waypoint
            if (transform.position == waypoints[index])
            {
                index++;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(FlashRed());
        life -= damage;

        if (life <= 0f)
        {
            dataManager.AddGold(gold);
            Destroy(gameObject);
        }
    }

    public void SlowDownEnemy(float slowValue)
    {
        StartCoroutine(SlowDown(slowValue));
    }

    public IEnumerator SlowDown(float slowEffect)
    {
        if(!slowed)
        {
            Debug.Log("Speed at begin " + speed);
            slowed = true;
            speed -= slowEffect;
            Debug.Log("Speed after slow " + speed);
            yield return new WaitForSeconds(3f);
            Debug.Log("Speed after yield " + speed);
            speed += slowEffect;
            slowed = false;
            Debug.Log("Speed at end of fct " + speed);
        }
        
    }

    public IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
