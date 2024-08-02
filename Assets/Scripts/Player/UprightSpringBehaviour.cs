using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UprightSpringBehaviour : MonoBehaviour
{
    public void LookAtPlayerForce(Vector3 forceVector)
    {
        transform.rotation = Quaternion.LookRotation(forceVector);
    }
}
