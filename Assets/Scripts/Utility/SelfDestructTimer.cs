using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    private float timer = 5f;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
            DestroyImmediate(gameObject);
    }
}
