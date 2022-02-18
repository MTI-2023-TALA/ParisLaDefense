using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public bool shouldGenerateMap = true;

    public float volume = 1;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag(Tag.option).Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
