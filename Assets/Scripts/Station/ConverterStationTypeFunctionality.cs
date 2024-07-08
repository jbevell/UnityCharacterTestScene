using System;
using UnityEngine;

public class ConverterStationTypeFunctionality : StationTypeFunctionality
{
	[SerializeField] private GameObject _spawnableObject;
	[SerializeField] private float _productionTimeLimit = 5f;

	private float _elapsedProductionTime = 0;
	private bool _playerInsertedObject = false;

	public Action OnProductionCompletedEvent;

	public bool IsProcessing => _playerInsertedObject == true;

	public ConverterStationTypeFunctionality(GameObject spawnableObject, float productionTimeLimit = 5f)
	{
		_spawnableObject = spawnableObject;
		_productionTimeLimit = productionTimeLimit;
	}

	// Start is called before the first frame update
	void Start()
	{
		OnProductionCompletedEvent += OnProductionComplete;
	}

	// Update is called once per frame
	void Update()
	{
		if (_playerInsertedObject)
		{
			_elapsedProductionTime += Time.deltaTime;

			Debug.Log("Converting process elapsed time: " + _elapsedProductionTime);

			while (_elapsedProductionTime >= _productionTimeLimit)
			{
				OnProductionCompletedEvent();
				_elapsedProductionTime = 0;
				_playerInsertedObject = false;
			}
		}
	}

	public void StartConverting(TaskObject taskObjectToDestroy = null)
	{
		_playerInsertedObject = true;
	}

	public void OnProductionComplete()
	{
		Instantiate(_spawnableObject, transform.position + transform.forward, new Quaternion());
	}

	private void OnDestroy()
	{
		OnProductionCompletedEvent -= OnProductionComplete;
	}
}
