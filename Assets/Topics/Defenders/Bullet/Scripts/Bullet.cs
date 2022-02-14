using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float damage;
    private bool attacksAOE;
    public float speed = 4f;
    public float slowEffect = 1f;

    public void Init(Transform _target, float _damage, bool _attacksAOE)
    {
        target = _target;
        damage = _damage;
        attacksAOE = _attacksAOE;
    }
    // Update is called once per frame
    void Update()
    {
        if(!target)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float curDist = speed * Time.deltaTime;

        if (dir.magnitude <= curDist)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * curDist, Space.World);
    }

    public void HitTarget()
    {
        if (attacksAOE)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tag.enemy);
            foreach (GameObject e in enemies)
            {
                Enemy enemy = (e.transform).GetComponent<Enemy>();
                float distanceBulletEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceBulletEnemy <= 2)
                {
                    enemy.TakeDamage(damage);
                    enemy.SlowDownEnemy(slowEffect);
                }
            }
        }

        else
        {
            Enemy realTarget = target.GetComponent<Enemy>();
            realTarget.TakeDamage(damage);
            realTarget.SlowDownEnemy(slowEffect);
        }

        Destroy(gameObject);
    }
}
