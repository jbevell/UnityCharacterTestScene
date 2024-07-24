using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerStationTypeFunctionality : StationTypeFunctionality
{
	public bool UseQueue = false;
	[HideInInspector] public int QueueLimit;

	private Queue<TaskObjectType> itemQueue;

	public Action<TaskObjectType> OnConsumption;

	private void Start()
	{
		if (UseQueue)
		{
			itemQueue = new Queue<TaskObjectType>();
		}
	}

	public void OnPlayerInteraction(TaskObject objectToConsume = null)
	{
		if (objectToConsume == null)
			return;

		if (UseQueue)
		{
			if (TryToStore(objectToConsume.TaskObjectType))
				objectToConsume.DestroyTaskObject();
		}
		else
		{
			objectToConsume.DestroyTaskObject();
		}
	}

	private bool TryToStore(TaskObjectType typeToStore)
	{
		bool storageSucceeded = false;

		if (UseQueue && itemQueue.Count < QueueLimit)
		{
			itemQueue.Enqueue(typeToStore);
			storageSucceeded = true;
		}

		return storageSucceeded;
	}

	public bool UseStoredItem()
	{
		TaskObjectType type;

		if (itemQueue.TryDequeue(out type))
		{
			OnConsumption(type);
			return true;
		}

		return false;
	}
}
