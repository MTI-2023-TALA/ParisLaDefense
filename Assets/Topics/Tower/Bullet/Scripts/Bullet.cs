using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float damage;
    public float speed = 4f;

    public void Init(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
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
        Enemy realTarget = target.GetComponent<Enemy>();
        realTarget.TakeDamage(damage);
        Destroy(gameObject);
    }
}
