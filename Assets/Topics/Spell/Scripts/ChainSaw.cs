using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSaw : MonoBehaviour
{
    private List<Vector3> waypoints;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 3f;

    private int index;

    public void Init(List<Vector3> waypoints)
    {
        this.waypoints = waypoints;
        index = waypoints.Count - 1;

        transform.position = waypoints[index];
    }

    private void Update()
    {
        MoveChainSaw();
    }

    private void MoveChainSaw()
    {
        // loop until last waypoint is reached 
        if (index == 0)
        {
            Destroy(this.gameObject);
        }
   
        // Move Enemy from current waypoint to the next one
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(waypoints[index].x, waypoints[index].y),
            speed * Time.deltaTime);

        // if reached next waypoint
        if (transform.position == waypoints[index])
        {
            index--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tag.enemy)
        {
            Enemy realTarget = collision.GetComponent<Enemy>();
            realTarget.TakeDamage(damage);
        }
    }
}
