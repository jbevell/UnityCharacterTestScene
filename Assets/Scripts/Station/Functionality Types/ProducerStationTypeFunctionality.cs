using UnityEngine;

public class ProducerStationTypeFunctionality : StationTypeFunctionality
{
	[SerializeField] private GameObject _spawnableObject;
	[SerializeField] private GameObject _spawnOriginObject;
	[SerializeField] private int _taskObjectLimit = 100;
	
	private int _taskObjectCurrentCount;

	private Vector3 _defaultSpawnPoint;
	private Quaternion _defaultSpawnRotation;
	private Vector3 _defaultExpulsionForce;

	// Currently only for idle spawning fun
	[SerializeField] public bool _streamIdle = false;
	private float _elapsedStreamIdleTime = 0;
	private float _timeBetweenEvents = 15;
	private Vector3 _spawnPosition;

	private void Start()
	{
		UpdateDefaultSpawnPosition();
		_defaultSpawnRotation = new Quaternion();
		UpdateDefaultExpulsionForce();
	}

	void Update()
	{
		if (_streamIdle)
		{
			_spawnPosition = _defaultSpawnPoint;
			_elapsedStreamIdleTime += Time.deltaTime;

			while (_elapsedStreamIdleTime >= _timeBetweenEvents)
			{
				_elapsedStreamIdleTime -= _timeBetweenEvents;
				ProduceItem(_spawnPosition, new Quaternion());
			}
		}
	}

	public void ProduceItem(Vector3? position = null, Quaternion? rotation = null, Vector3? expulsionForce = null)
	{
		if (position == null)
		{
			UpdateDefaultSpawnPosition();
			position = _defaultSpawnPoint;
		}

		if (rotation == null)
			rotation = _defaultSpawnRotation;

		if (expulsionForce == null)
		{
			UpdateDefaultExpulsionForce();
			expulsionForce = _defaultExpulsionForce;
		}
		
		Debug.Log("Player interacted with station!");

		if (_taskObjectCurrentCount < _taskObjectLimit)
		{
			_taskObjectCurrentCount++;
			GameObject createdObject = Instantiate(_spawnableObject, (Vector3)position, (Quaternion)rotation);
			createdObject.GetComponent<Rigidbody>().AddForce((Vector3)expulsionForce, ForceMode.Impulse);
		}
		else
		{
			Debug.Log("Station is out of supplies");
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
