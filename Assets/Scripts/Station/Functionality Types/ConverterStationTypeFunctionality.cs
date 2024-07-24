using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: Allow converters to accept different inputs and provide multiple outputs
public class ConverterStationTypeFunctionality : StationTypeFunctionality
{
	[SerializeField] private GameObject _spawnableObject;
	[SerializeField] private GameObject _spawnOriginObject;
	[SerializeField] private float _productionTimeLimit = 5f;
	[SerializeField] private GameObject[] _acceptedTaskObjects;

	private float _elapsedProductionTime = 0;
	private bool _playerInsertedObject = false;

	private Vector3? _resultSpawnPosition;
	private Quaternion? _resultRotation;
	private Vector3? _expulsionForce;

	private Vector3 _defaultSpawnPoint;
	private Quaternion _defaultSpawnRotation;
	private Vector3 _defaultExpulsionForce;

	public Action OnProductionCompletedEvent;

	public TaskObjectType[] AcceptedTaskObjectTypes => _acceptedTaskObjects.Select(x => x.GetComponentInChildren<TaskObject>().TaskObjectType).Distinct().ToArray();
	public bool IsProcessing => _playerInsertedObject == true;
	public bool UseDefaultSpawnPosition => _resultSpawnPosition == null;
	public bool UseDefaultRotation => _resultRotation == null;
	public bool UseDefaultForce => _expulsionForce == null;
	
	// Currently only for idle spawning fun
	[SerializeField] bool _useStreamIdle = false;

	// Start is called before the first frame update
	void Start()
	{
		UpdateDefaultSpawnPosition();
		_defaultSpawnRotation = new Quaternion();
		UpdateDefaultExpulsionForce();
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
				if (OnProductionCompletedEvent != null)
					OnProductionCompletedEvent();

				OnConversionComplete();
				_elapsedProductionTime = 0;
				_playerInsertedObject = false;
			}
		}
	}

	public void AttemptStreamIdleEffect(Collider other)
	{
		if (!_useStreamIdle)
			return;

		if (other.transform.CompareTag(Tags.TaskObject.ToString()) && !IsProcessing)
		{
			TaskObject taskObject = other.GetComponent<TaskObject>();
			TaskObjectType type = taskObject.TaskObjectType;

			switch (type)
			{
				case TaskObjectType.Ball:
					StartConverting();
					taskObject.DestroyTaskObject();
					break;
			}
		}
	}

	public void StartConverting(Vector3? resultSpawnPosition = null, Quaternion? resultRotation = null, Vector3? expulsionForce = null)
	{
		_resultSpawnPosition = resultSpawnPosition;
		_resultRotation = resultRotation;
		_expulsionForce = expulsionForce;
		_playerInsertedObject = true;
	}

	public void OnConversionComplete()
	{
		AttemptToUpdateSpawnValuesToDefault();

		GameObject createdObject = Instantiate(_spawnableObject, (Vector3)_resultSpawnPosition, (Quaternion)_resultRotation);
		createdObject.GetComponent<Rigidbody>().AddForce((Vector3)_expulsionForce, ForceMode.Impulse);
	}

	private void AttemptToUpdateSpawnValuesToDefault()
	{
		if (UseDefaultSpawnPosition)
		{
			UpdateDefaultSpawnPosition();
			_resultSpawnPosition = _defaultSpawnPoint;
		}

		if (UseDefaultRotation)
			_resultRotation = _defaultSpawnRotation;

		if (UseDefaultForce)
		{
			UpdateDefaultExpulsionForce();
			_expulsionForce = _defaultExpulsionForce;
		}
	}

	private void UpdateDefaultSpawnPosition()
	{
		_defaultSpawnPoint = _spawnOriginObject.transform.position + (_spawnOriginObject.transform.forward * 2);
	}

	private void UpdateDefaultExpulsionForce()
	{
		_defaultExpulsionForce = transform.forward;
	}
}
