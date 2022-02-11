using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 3f;

    private Vector3 ballStartPos;
    private Vector3 ballEndPos;

    private void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tag.enemy);
        ballStartPos = new Vector3(1f, 25f);
        ballEndPos = new Vector2(1f, -25f);

        if (enemies.Length > 0)
        {
            ballStartPos.x = enemies[0].transform.position.x;
            ballEndPos.x = enemies[0].transform.position.x;
        }

        transform.position = ballStartPos;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, ballEndPos, speed * Time.deltaTime);
        if (transform.position == ballEndPos)
        {
            Destroy(this.gameObject);        
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
