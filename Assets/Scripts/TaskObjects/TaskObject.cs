using UnityEngine.Events;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.AI;

public class TaskObject : MonoBehaviour, IInteractive
{
	[SerializeField] protected GameObject _interactionInstructionObject;
	[SerializeField] protected GameObject _mainObject;
	[SerializeField] protected Rigidbody _taskObjectRigidBody;

	protected MeshRenderer _interactionInstructionRenderer;
	protected Collider _playerDetectionZone;
	protected Camera _cameraReference;

	protected TaskObjectType _taskObjectType;
	protected bool _isCarried = false;
	
	public TaskObjectType TaskObjectType => _taskObjectType;

	protected void Start()
	{
		_interactionInstructionRenderer = _interactionInstructionObject.GetComponent<MeshRenderer>();
		_interactionInstructionObject.GetComponent<ObjectLookAt>().ObjectToLookAt = FindObjectOfType<Camera>().gameObject;
		_playerDetectionZone = GetComponent<Collider>();

		_interactionInstructionRenderer.enabled = false;
	}

	// TODO: Prevent grabbing multiple objects
	// TODO: Prevent missing event assignment
	public virtual void OnPlayerEntered(Player player)
	{
		if (player.IsHoldingObject)
			return;

		Debug.Log("Entering task object trigger");

		OnPlayerEnteredTriggerCheck(player);
	}

	private void OnPlayerEnteredTriggerCheck(Player player)
	{
		bool triggersMatch = player.PrioritizedTrigger == _playerDetectionZone;
		Debug.Log("Do triggers match for this trigger enter? " + triggersMatch);

		if (triggersMatch)
		{
			////player.OnObjectPickUpEvent += OnPlayerInteraction;
			_interactionInstructionRenderer.enabled = true;
			Debug.LogWarning("Enabling E on trigger enter for " + transform.parent.name);
		}
		else
		{
			////player.OnObjectPickUpEvent -= OnPlayerInteraction;
			_interactionInstructionRenderer.enabled = false;

			Debug.LogWarning("Turning off E on enter for " + transform.parent.name);
		}
	}

	public virtual void OnPlayerStay(Player player)
	{
		
	}
	
	public virtual void OnPlayerExited(Player player)
	{
		Debug.Log("Exiting task object trigger");

		////if (player.PrioritizedTrigger == _playerDetectionZone)
		{
			////player.OnObjectPickUpEvent -= OnPlayerInteraction;
			_interactionInstructionRenderer.enabled = false;
			Debug.LogWarning("Turning off E on exit for " + transform.parent.name);
		}
	}

	public ObjectInteractions OnPlayerInteraction(TaskObject playersCurrentHeldObject)
	{
		if (playersCurrentHeldObject != null)
			return ObjectInteractions.NoAction;

		Player player = FindObjectOfType<Player>();

		Debug.Log("Task Object player has interacted with " + gameObject.name);

		_isCarried = true;
		player.OnObjectDropEvent += OnPlayerDropped;

		//TODO: This reference will need to be updated to a global object manager at some point to facilitate multiplayer.
		ChangeObjectParent(player.transform, player.transform.position + player.transform.forward);

		_playerDetectionZone.enabled = false;
		_interactionInstructionRenderer.enabled = false;
		Debug.LogWarning("Turning off E on interaction for " + transform.parent.name);
		_taskObjectRigidBody.isKinematic = true;

		return ObjectInteractions.TaskObjectPickUp;
	}

	////public virtual void OnPlayerInteraction(Player player)
	////{
	////	Debug.Log("Task Object player has interacted with " + gameObject.name);

	////	_isCarried = true;
	////	player.OnObjectDropEvent += OnPlayerDropped;
	////	player.OnObjectPickUpEvent -= OnPlayerInteraction;

	////	//TODO: This reference will need to be updated to a global object manager at some point to facilitate multiplayer.
	////	ChangeObjectParent(player.transform, player.transform.position + player.transform.forward);

	////	_playerDetectionZone.enabled = false;
	////	_interactionInstructionRenderer.enabled = false;
	////	Debug.LogWarning("Turning off E on interaction for " + transform.parent.name);
	////	_taskObjectRigidBody.isKinematic = true;
	////}

	public virtual void OnPlayerDropped(Player player)
	{
		if (!_isCarried)
			return;

		_isCarried = false;
		player.OnObjectDropEvent -= OnPlayerDropped;
		////player.OnObjectPickUpEvent -= OnPlayerInteraction;

		ChangeObjectParent(null, transform.position);

		_playerDetectionZone.enabled = true;
		_interactionInstructionRenderer.enabled = false;
		Debug.LogWarning("Turning off E on drop for " + transform.parent.name);
		_taskObjectRigidBody.isKinematic = false;
	}

	public void ChangeObjectParent(Transform newParent, Vector3 relativePosition)
	{
		_mainObject.transform.SetParent(newParent, true);
		_mainObject.transform.SetPositionAndRotation(relativePosition, new Quaternion());
		_mainObject.transform.localScale = Vector3.one;
	}

	public void DestroyTaskObject()
	{
		Destroy(_mainObject);
	}
}
