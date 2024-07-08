using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BreadstickConverter : Station
{
	[SerializeField] private GameObject spawnableObjectPrefab;
	[SerializeField] private float _productionTimeLimit = 5f;

	ConverterStationTypeFunctionality converterComponent;

	private void Start()
	{
		converterComponent = GetComponent<ConverterStationTypeFunctionality>();
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
