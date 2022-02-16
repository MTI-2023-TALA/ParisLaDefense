using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyUtility : MonoBehaviour
{
    [SerializeField] private float second = 1f;
    void Start()
    {
        StartCoroutine(destroyAfterXSecond());
    }

    IEnumerator destroyAfterXSecond()
    {
        yield return new WaitForSeconds(second);
        Destroy(this.gameObject);
    }
}
