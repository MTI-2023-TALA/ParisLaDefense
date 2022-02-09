using UnityEngine;

public class FollowRoad : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform[] waypoints;
    private int index = 0;

    private void Start()
    {
        // Set position of Enemy as position of the first waypoint
        transform.position = waypoints[index].transform.position;
    }

    private void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        // loop until last waypoint is reached
        if (index < waypoints.Length)
        {
            // Move Enemy from current waypoint to the next one
            transform.position = Vector2.MoveTowards(transform.position,
               waypoints[index].transform.position,
               speed * Time.deltaTime);

            // if reached next waypoint
            if (transform.position == waypoints[index].transform.position)
            {
                index++;
            }
        }
    }
}