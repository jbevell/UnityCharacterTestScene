using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTestObject : MonoBehaviour
{
    public QuaternionTest testQuaternionInfo;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = testQuaternionInfo.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(testQuaternionInfo.testVector);
    }
}
