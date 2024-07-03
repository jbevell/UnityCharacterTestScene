using UnityEngine;

public class BreadstickConverter : Station
{
	[SerializeField] private float _productionTimeLimit = 5f;

	private float _elapsedProductionTime = 0;
	private bool _playerInsertedObject = false;

    // Start is called before the first frame update
    void Start()
    {
        _stationType = StationType.Converter;
    }

    // Update is called once per frame
    void Update()
    {
		if (_playerInsertedObject)
		{
			_elapsedProductionTime += Time.deltaTime;

			while (_elapsedProductionTime >= _productionTimeLimit)
			{
				_taskObjectSpawnHandler.MakeBread(transform.position + transform.forward, new Quaternion());
				_elapsedProductionTime = 0;
				_playerInsertedObject = false;
			}
		}
    }

	public override bool OnPlayerInteraction(TaskObject playersHeldObject = null)
	{
        if (playersHeldObject == null)
            return false;

		return TakeTaskObject(playersHeldObject);
	}

	public bool TakeTaskObject(TaskObject taskObject)
	{
		if (taskObject.TaskObjectType == TaskObjectType.Ball)
		{
			taskObject.ChangeObjectParent(transform, taskObject.transform.position);
			taskObject.DestroyTaskObject();
			_playerInsertedObject = true;
			return true;
		}

		Debug.Log("I can't accept this type");
		return false;
	}
}
