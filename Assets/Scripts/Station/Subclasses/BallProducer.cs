using UnityEngine;

public class BallProducer : Station
{
	[SerializeField] private GameObject spawnableObjectPrefab;

	ProducerStationTypeFunctionality producerComponent;

	// Start is called before the first frame update
	void Start()
	{
		producerComponent = GetComponent<ProducerStationTypeFunctionality>();
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject taskObject = null)
	{
		producerComponent.ProduceItem(transform.position + transform.forward, new Quaternion());
		return ObjectInteractions.NoAction;
	}
}
