using System.Linq;
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

	private void OnTriggerEnter(Collider other)
	{
		if (converterComponent == null)
			return;

		converterComponent.AttemptStreamIdleEffect(other);
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject taskObject = null)
	{
		if (taskObject != null)
		{
			if (converterComponent.AcceptedTaskObjectTypes.Contains(taskObject.TaskObjectType))
			{
				if (!converterComponent.IsProcessing)
				{
					converterComponent.StartConverting();
					taskObject.DestroyTaskObject();
					return ObjectInteractions.StationTake;
				}
			}
		}

		producerComponent.ProduceItem(transform.position + (transform.forward * 2f), new Quaternion());
		return ObjectInteractions.NoAction;
	}
}
