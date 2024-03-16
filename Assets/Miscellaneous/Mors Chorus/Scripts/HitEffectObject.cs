using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectObject : MonoBehaviour
{
    public float lifetime = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {

    }
}
