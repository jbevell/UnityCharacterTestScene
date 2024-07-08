using System;
using UnityEngine;
using UnityEngine.Scripting;

public abstract class Station : MonoBehaviour, IInteractive
{
	[SerializeField] protected MeshRenderer _interactionInstructionRenderer;
	[SerializeField] protected Collider _playerDetectionZone;

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

	public abstract ObjectInteractions OnPlayerInteraction(TaskObject playerHeldTaskObject);
}