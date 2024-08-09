using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageLocker : Station
{
    private StorageStationTypeFunctionality storageComponent;

    // Start is called before the first frame update
    void Start()
    {
        storageComponent = GetComponent<StorageStationTypeFunctionality>();
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject playerHeldTaskObject)
	{
		if (!storageComponent.OnPlayerInteraction(playerHeldTaskObject))
			return ObjectInteractions.NoAction;

		return ObjectInteractions.StationTake;
	}
}
