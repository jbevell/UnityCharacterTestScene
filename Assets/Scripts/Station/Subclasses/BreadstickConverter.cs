using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BreadstickConverter : Station
{
	ConverterStationTypeFunctionality converterComponent;

	private void Start()
	{
		converterComponent = GetComponent<ConverterStationTypeFunctionality>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (converterComponent == null)
			return;

		converterComponent.AttemptStreamIdleEffect(other);
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject playersHeldObject = null)
	{
		if (playersHeldObject == null)
		{
			Debug.Log("I cannot accept that type");
			return ObjectInteractions.NoAction;
		}

		switch (playersHeldObject.TaskObjectType)
		{
			case TaskObjectType.Ball:
				converterComponent.StartConverting();
				playersHeldObject.DestroyTaskObject();
				return ObjectInteractions.StationTake;
			default:
				Debug.Log("I cannot accept that type");
				return ObjectInteractions.NoAction;
		}
	}
}
