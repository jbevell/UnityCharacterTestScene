using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Station
{
    private ConsumerStationTypeFunctionality consumerComponent;

	private void Start()
	{
		consumerComponent = GetComponent<ConsumerStationTypeFunctionality>();
		consumerComponent.UseQueue = false;
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject playerHeldTaskObject)
    {
		consumerComponent.OnPlayerInteraction(playerHeldTaskObject);

		return ObjectInteractions.StationTake;
    }
}
