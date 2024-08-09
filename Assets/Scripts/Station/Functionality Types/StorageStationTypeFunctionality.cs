using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageStationTypeFunctionality : StationTypeFunctionality
{
	public Action<TaskObject> OnConsumption;

	[SerializeField] private int QueueLimit;
	private Queue<TaskObject> itemQueue;

	private void Start()
	{
		itemQueue = new Queue<TaskObject>();
	}

	public bool OnPlayerInteraction(TaskObject objectToConsume = null, Vector3? spawnPosition = null)
	{
		if (spawnPosition == null)
			spawnPosition = transform.position + transform.forward;

		if (objectToConsume == null)
		{
			WithdrawItem((Vector3)spawnPosition);
			return true;
		}

		if (TryToStore(objectToConsume))
		{
			objectToConsume.HideTaskObject(transform, transform.position);
			return true;
		}
		else
			return false;
	}

	private bool TryToStore(TaskObject objectToStore)
	{
		if (itemQueue.Count >= QueueLimit)
			return false;

		itemQueue.Enqueue(objectToStore);

		return true;
	}

	public bool UseStoredItem()
	{
		TaskObject taskObject;

		if (itemQueue.TryDequeue(out taskObject))
		{
			if (OnConsumption != null)
				OnConsumption(taskObject);

			return true;
		}

		return false;
	}

	public void WithdrawItem(Vector3 spawnPosition)
	{
		if (itemQueue.Count == 0)
			return;

		TaskObject taskObject = itemQueue.Dequeue();
		taskObject.ShowTaskObject(transform, spawnPosition);
	}
}
