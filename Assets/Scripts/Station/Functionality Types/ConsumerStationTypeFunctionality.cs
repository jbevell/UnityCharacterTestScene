using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerStationTypeFunctionality : StationTypeFunctionality
{
	public bool UseQueue = false;
	[HideInInspector] public int QueueLimit;

	private Queue<TaskObject> itemQueue;

	public Action<TaskObject> OnConsumption;

	private void Start()
	{
		if (UseQueue)
		{
			itemQueue = new Queue<TaskObject>();
		}
	}

	public void OnPlayerInteraction(TaskObject objectToConsume = null)
	{
		if (UseQueue)
		{
			if (objectToConsume == null)
				WithdrawItem();
			else if (TryToStore(objectToConsume))
				objectToConsume.DestroyTaskObject();
		}
		else
		{
			if (objectToConsume == null)
				return;

			objectToConsume.DestroyTaskObject();
		}
	}

	private bool TryToStore(TaskObject objectToStore)
	{
		bool storageSucceeded = false;

		if (UseQueue && itemQueue.Count < QueueLimit)
		{
			itemQueue.Enqueue(objectToStore);
			storageSucceeded = true;
		}

		return storageSucceeded;
	}

	public bool UseStoredItem()
	{
		TaskObject taskObject;

		if (itemQueue.TryDequeue(out taskObject))
		{
			OnConsumption(taskObject);
			return true;
		}

		return false;
	}

	public void WithdrawItem()
	{
		if (itemQueue.Count == 0)
			return;

		TaskObject taskObject = itemQueue.Dequeue();

		Instantiate(taskObject);
	}
}
