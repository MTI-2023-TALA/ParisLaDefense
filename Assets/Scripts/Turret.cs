using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Turret attributes")]
    public float range = 4f;
    public float turnSpeed = 10f;
    public float fireRate = 1f;
    public float fireCoutdown = 0f;
    public float damage = 1f;
    public GameObject bullet;

    [Header("Enemy attributes")]
    public string enemyTag = "Enemy";
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDist = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach(GameObject enemy in enemies)
        {
            float curDist = Vector2.Distance(transform.position, enemy.transform.position);
            if(curDist < shortestDist)
            {
                shortestDist = curDist;
                closestEnemy = enemy;
            }
        }
        if(closestEnemy != null && shortestDist <= range)
        {
            target = closestEnemy.transform;
        } else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector2 dirToEnemy = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToEnemy);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation.z);

        //Can fire
        if(fireCoutdown <= 0f)
        {
            Shoot();
            fireCoutdown = 1f / fireRate;
        }

        fireCoutdown -= Time.deltaTime;
    }

    void Shoot()
    {
        Bullet bulletSent = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>();
        bulletSent.Init(target, damage);
    }

    //DEBUG
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
