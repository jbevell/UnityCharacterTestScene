using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UprightSpringBehaviour : MonoBehaviour
{
    SpringJoint _springJoint;

    private void Start()
    {
        _springJoint = GetComponent<SpringJoint>();
    }

    public void LookAtPlayerForce(Vector3 forceVector)
    {
        transform.rotation = Quaternion.LookRotation(forceVector);
    }

    public void BreakSpring()
    {
        Destroy(_springJoint);
    }
}
