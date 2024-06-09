using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject PlayerObject;

    private Vector3 cameraOffset;// = new Vector3(0, 5, -10);
    private float cameraMaxSpeed = 10;

	// Start is called before the first frame update
	void Start()
    {
        cameraOffset = transform.position - PlayerObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = Vector3.MoveTowards(transform.position, PlayerObject.transform.position + cameraOffset, cameraMaxSpeed);
    }
}
