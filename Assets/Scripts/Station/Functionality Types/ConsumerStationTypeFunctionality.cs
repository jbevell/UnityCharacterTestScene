using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

	public bool OnPlayerInteraction(TaskObject objectToConsume = null, Vector3? spawnPosition = null)
	{
		if (UseQueue)
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
		else
		{
			if (objectToConsume == null)
				return false;

			objectToConsume.DestroyTaskObject();
			return true;
		}
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
