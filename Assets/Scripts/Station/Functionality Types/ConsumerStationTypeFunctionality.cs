using System;
using UnityEngine;

public class ConsumerStationTypeFunctionality : StationTypeFunctionality
{
	public Action<TaskObject> OnConsumption;

	public bool OnPlayerInteraction(TaskObject objectToConsume = null)
	{
		if (objectToConsume == null)
			return false;

		if(OnConsumption != null)
			OnConsumption(objectToConsume);

		objectToConsume.DestroyTaskObject();

		return true;
	}
}
