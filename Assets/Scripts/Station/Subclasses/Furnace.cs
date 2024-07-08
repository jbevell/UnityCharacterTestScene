using UnityEngine;

public class Furnace : Station
{
	ConverterStationTypeFunctionality converterComponent;
	ProducerStationTypeFunctionality producerComponent;

	// Start is called before the first frame update
	void Start()
	{
		converterComponent = GetComponent<ConverterStationTypeFunctionality>();
		producerComponent = GetComponent<ProducerStationTypeFunctionality>();
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject taskObject = null)
	{
		if (taskObject != null)
		{
			switch (taskObject.TaskObjectType)
			{
				case TaskObjectType.Ball:
					if (!converterComponent.IsProcessing)
					{
						converterComponent.StartConverting();
						taskObject.DestroyTaskObject();
						return ObjectInteractions.StationTake;
					}
					else
						break;
				default:
					break;
			}
		}

		producerComponent.ProduceItem(transform.position + transform.forward, new Quaternion());
		return ObjectInteractions.NoAction;
	}
}
