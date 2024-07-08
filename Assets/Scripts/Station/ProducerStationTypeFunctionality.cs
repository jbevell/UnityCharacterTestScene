using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProducerStationTypeFunctionality : StationTypeFunctionality
{
	[SerializeField] protected Station _parentObjectStation;
	[SerializeField] private GameObject _spawnableObject;
	[SerializeField] private int _taskObjectLimit = 100;
	
	int _taskObjectCurrentCount;

	// Currently only for idle spawning fun
	[SerializeField] public bool _streamIdle = true;
	private float _elapsedStreamIdleTime = 0;
	private float _timeBetweenEvents = 15;
	private Vector3 _spawnPosition;

	public ProducerStationTypeFunctionality(Station stationOfImplementation, GameObject spawnableObject, int taskObjectTotalLimit = 100)
	{
		_parentObjectStation = stationOfImplementation;
		_spawnableObject = spawnableObject;
		_taskObjectLimit = taskObjectTotalLimit;
	}

	// Start is called before the first frame update
	void Start()
	{
		if (_streamIdle)
		{
			_spawnPosition = _parentObjectStation.transform.position + _parentObjectStation.transform.forward;
			_elapsedStreamIdleTime += Time.deltaTime;

			while (_elapsedStreamIdleTime >= _timeBetweenEvents)
			{
				_elapsedStreamIdleTime -= _timeBetweenEvents;
				ProduceItem(_spawnPosition, new Quaternion());
			}
		}
	}

	public void ProduceItem(Vector3 position, Quaternion rotation)
	{
		Debug.Log("Player interacted with station!");

		if (_taskObjectCurrentCount < _taskObjectLimit)
		{
			_taskObjectCurrentCount++;
			
			//TODO Reconsider ambiguity between this generic instantiation and the static spawner
			Instantiate(_spawnableObject, position, rotation);
		}
		else
		{
			Debug.Log("Station is out of supplies");
		}
	}
}
