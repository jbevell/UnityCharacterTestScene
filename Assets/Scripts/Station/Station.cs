using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] protected MeshRenderer _interactionInstructionRenderer;
    [SerializeField] protected Collider _playerDetectionZone;
    [SerializeField] protected GameObject _spawnableObject;
	[SerializeField] protected TaskObjectSpawnUtility _taskObjectSpawnHandler;
	[SerializeField] protected int _taskObjectLimit;

	protected int _taskObjectCurrentCount;
    protected StationType _stationType;

    // Currently only for idle spawning fun
    [SerializeField] bool _streamIdle = false;
    float _elapsedStreamIdleTime = 0;
    float _timeBetweenEvents = 15;

    public StationType StationObjectType => _stationType;

	private void Update()
	{
        if (_streamIdle)
        {
            _elapsedStreamIdleTime += Time.deltaTime;

            while (_elapsedStreamIdleTime >= _timeBetweenEvents)
            {
                _elapsedStreamIdleTime -= _timeBetweenEvents;
                OnPlayerInteraction();
            }
        }
	}

	public void OnPlayerEntered()
    {
		Debug.Log("Firing station enter method");
		_interactionInstructionRenderer.enabled = true;
    }

    public void OnPlayerExited()
    {
		Debug.Log("Player has left station space");
		_interactionInstructionRenderer.enabled = false;
    }

	public virtual bool OnPlayerInteraction(TaskObject playersHeldTaskObject = null)
    {
		Debug.Log("Player interacted with station!");

		if (_taskObjectCurrentCount < _taskObjectLimit)
        {
			_taskObjectCurrentCount++;
			Instantiate(_spawnableObject, transform.position + transform.forward, new Quaternion(0, 0, 0, 0));
		}
        else
        {
			Debug.Log("Station is out of supplies");
		}

        return false;
    }
}
