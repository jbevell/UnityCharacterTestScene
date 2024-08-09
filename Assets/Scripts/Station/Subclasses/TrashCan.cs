public class TrashCan : Station
{
    private ConsumerStationTypeFunctionality consumerComponent;

	private void Start()
	{
		consumerComponent = GetComponent<ConsumerStationTypeFunctionality>();
		//consumerComponent.UseQueue = false;
	}

	public override ObjectInteractions OnPlayerInteraction(TaskObject playerHeldTaskObject)
    {
		if (!consumerComponent.OnPlayerInteraction(playerHeldTaskObject))
			return ObjectInteractions.NoAction;

		return ObjectInteractions.StationTake;
    }
}
