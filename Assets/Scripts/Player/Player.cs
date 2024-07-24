using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string _playerID;
    private TaskObject _heldObject = null;

	private Collider _currentFocusedTrigger = null;
	private float _currentTriggerDistance = 100f;

	public Action<Player> OnObjectPickUpEvent;
	public Action<Player> OnObjectDropEvent;

	public string ID => _playerID;
    public TaskObject HeldObject => _heldObject;
    public bool IsHoldingObject => _heldObject != null;
	public Collider PrioritizedTrigger => _currentFocusedTrigger;

	void Update()
	{
		if (_currentFocusedTrigger != null)
			Debug.DrawLine(transform.position, _currentFocusedTrigger.transform.position, Color.green);

		if (Input.GetKeyDown(KeyCode.E) && _currentFocusedTrigger != null)
		{
			Transform targetTransform = _currentFocusedTrigger.transform;

			// Station
			////if (targetTransform.CompareTag(Tags.Station.ToString()))
			////{
			MonoBehaviour[] temp = _currentFocusedTrigger.GetComponents<MonoBehaviour>();

			foreach(MonoBehaviour mono in temp)
			{
				if (mono as IInteractive != null)
				{
					switch(((IInteractive)mono).OnPlayerInteraction(_heldObject))
					{
						case ObjectInteractions.StationTake:
							OnObjectTaken();
							break;
						case ObjectInteractions.TaskObjectPickUp:
							PickUpItem(mono as TaskObject);
							break;
						default:
							break;
					}
				}
			}

				////if (_currentFocusedTrigger.GetComponent<IStationInteractive>().OnPlayerInteraction(_heldObject))
				////	OnObjectTaken();
			////}

			// Task Object
			////if (targetTransform.CompareTag(Tags.TaskObject.ToString()))
				/////PickUpItem(_currentFocusedTrigger.GetComponent<TaskObject>());
		}

		if (Input.GetKeyDown(KeyCode.X) && IsHoldingObject)
		{
			DropHeldItem();
		}
	}

	////private void OnStationInteraction(Station station)
	////{
	////switch(station.StationObjectType)
	////{
	////	case StationType.BreadstickConverter:
	////		station.GetComponent<BreadstickConverter>().OnPlayerInteraction(_heldObject);
	////		break;
	////	case StationType.BallProducer:
	////		station.GetComponent<BallProducer>().OnPlayerInteraction(_heldObject);
	////		break;
	////}
	////}

	private void OnObjectTaken()
	{
		_heldObject = null;
		OnObjectPickUpEvent = null;
		OnObjectDropEvent = null;
	}

	private void OnTriggerEnter(Collider other)
	{
		Transform targetTransform = other.transform;

		////Debug.Log("Current target transform tag: " + targetTransform.name);

		// TODO resolve redundant task object tag comparison for trigger update
		UpdateCurrentTrigger(other);

		// Task Object
		if (targetTransform.CompareTag(Tags.TaskObject.ToString()) && !IsHoldingObject)
				other.GetComponent<TaskObject>().OnPlayerEntered(this);
		
		// Station
		if (targetTransform.CompareTag(Tags.Station.ToString()))
			other.GetComponent<Station>().OnPlayerEntered();
	}

	private void OnTriggerStay(Collider other)
	{
		UpdateCurrentTrigger(other);

		// Task Object
		if (other.transform.CompareTag(Tags.TaskObject.ToString()) && !IsHoldingObject)
			other.GetComponent<TaskObject>().OnPlayerStay(this);
	}

	private void OnTriggerExit(Collider other)
	{
		Transform targetTransform = other.gameObject.transform;

		// Task Object
		if (targetTransform.CompareTag(Tags.TaskObject.ToString()))
			other.GetComponent<TaskObject>().OnPlayerExited(this);

		// Station
		if (targetTransform.CompareTag(Tags.Station.ToString()))
			other.GetComponent<Station>().OnPlayerExited();

		if (other == _currentFocusedTrigger)
		{
			_currentFocusedTrigger = null;
			_currentTriggerDistance = 100f;
		}
	}

	// TODO: Resolve current trigger's distance not being updated even when greater for accurate comparison
	private void UpdateCurrentTrigger(Collider trigger)
	{
		if (trigger.transform.CompareTag(Tags.TaskObject.ToString()) && IsHoldingObject)
			return;

		float tempTriggerDistance = Vector3.Distance(transform.position, trigger.transform.position);
		bool lessThanCurrentDistance = tempTriggerDistance < _currentTriggerDistance;
		string lessThanString = lessThanCurrentDistance ? "less than" : "greater than";

		////Debug.Log($"Distance to trigger {trigger.transform.parent.name} is {tempTriggerDistance} which is {lessThanString} the current value of {_currentTriggerDistance}");

		if (tempTriggerDistance < _currentTriggerDistance)
		{
			////Debug.Log($"Setting current trigger to {trigger.transform.parent.name}");
			_currentFocusedTrigger = trigger;
			_currentTriggerDistance = tempTriggerDistance;
		}
		else if (trigger == _currentFocusedTrigger)
		{
			_currentTriggerDistance = tempTriggerDistance;
		}
	}

	public void PickUpItem(TaskObject item)
	{
		if (IsHoldingObject)
			return;

		_heldObject = item;
		_currentFocusedTrigger = null;
		_currentTriggerDistance = 100;
		Debug.Log("Current held object: " + _heldObject);

		////OnObjectPickUpEvent(this);
	}

	public void DropHeldItem()
	{
		if (!IsHoldingObject)
			return;

		Debug.Log("Dropping item " + _heldObject.TaskObjectType);

		OnObjectDropEvent(this);
		_heldObject = null;
	}
}
