using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Consider Storage Type station implementation
public abstract class StationTypeFunctionality : MonoBehaviour
{
	public Action OnPlayerEnteredEvent;
	public Action OnPlayerExitedEvent;

	////public abstract void OnPlayerEntered();

	////public abstract void OnPlayerExit();

	//TODO determine if there's any way to create a generic contract for these function variants
	////public void OnPlayerInteraction();
	////public void OnPlayerInteraction(Vector3, Quaternion);
	////public bool OnPlayerInteraction(TaskObject);
}
