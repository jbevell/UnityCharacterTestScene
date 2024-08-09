using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _throwableObjectPrefab;
    private bool _launch = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _launch = true;
        }
    }

	private void FixedUpdate()
	{
        if (_launch)
            LaunchObject();
	}

    private void LaunchObject()
    {
        _launch = false;

        GameObject throwableObject = Instantiate(_throwableObjectPrefab, transform.position + (transform.forward * 2), transform.rotation);
        Rigidbody throwBody = throwableObject.GetComponent<Rigidbody>();

        if (throwableObject.GetComponent<Rigidbody>() != null)
            throwBody = throwableObject.GetComponentInChildren<Rigidbody>();

        throwBody.AddForce(transform.forward * 30f, ForceMode.Impulse);
	}
}
