using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Transform target;

    [Header("Turret attributes")]
    public float range = 4f;
    public float fireRate = 1f;
    public float fireCountdown = 0f;
    public float damage = 1f;
    public int cost = 50;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tag.enemy);
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
        } else {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireCountdown -= Time.deltaTime;
        if (target == null)
        {
            return;
        }

        // if enemy is left of defender, do a left flip
        if (transform.position.x > target.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        // flip right
        else
            transform.rotation = Quaternion.Euler(0, 180f, 0);

        // Can fire
        if(fireCountdown <= 0f)
        {
            animator.SetBool("IsAttacking", true);
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
            {
                animator.SetBool("IsAttacking", false);
            }
        }
    }


    void Shoot()
    {
        Bullet bulletSent = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>();
        bulletSent.Init(target, damage);
    }
}
