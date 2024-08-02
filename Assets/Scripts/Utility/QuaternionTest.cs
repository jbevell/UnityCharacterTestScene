using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuaternionTest : MonoBehaviour
{
    public Vector3 testVector;
    public float sumOfSquares;
    public float realPart;
    public float i;
    public float j;
    public float k;

    // Start is called before the first frame update
    void Start()
    {
        testVector = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        ////float rotation = Random.Range(0, 360);
        realPart = Mathf.Clamp(realPart, -1, 1);
        i = Mathf.Clamp(i, -1, 1);
        j = Mathf.Clamp(j, -1, 1);
        k = Mathf.Clamp(k, -1, 1);

		sumOfSquares = realPart * realPart + i * i + j * j + k * k;

		if (sumOfSquares > 1 || sumOfSquares < -1)
        {

        }

        testVector = Vector3.forward * 5;
        testVector = new Quaternion(i, j, k, realPart) * testVector;
        ////testVector = Quaternion.Euler(rotation, rotation, rotation) * testVector;
    }

    private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + testVector);
	}
}
