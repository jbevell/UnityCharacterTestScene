using UnityEngine;

public class BallProducer : Station
{
	ProducerStationTypeFunctionality producerComponent;

	// Start is called before the first frame update
	void Start()
	{
		producerComponent = GetComponent<ProducerStationTypeFunctionality>();
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject taskObject = null)
	{
		producerComponent.ProduceItem(expulsionForce: transform.forward * 5);
		return ObjectInteractions.NoAction;
	}
}
