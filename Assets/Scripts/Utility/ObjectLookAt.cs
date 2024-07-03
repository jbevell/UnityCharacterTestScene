using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLookAt : MonoBehaviour
{
    public GameObject ObjectToLookAt;
    public Vector3 LookRotationAdjustment;

    // Update is called once per frame
    void Update()
    {
        if (ObjectToLookAt == null)
            return;

        transform.LookAt(ObjectToLookAt.transform);
        transform.Rotate(LookRotationAdjustment);
    }
}
