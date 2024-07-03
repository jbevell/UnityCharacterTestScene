public class BallTaskObject : TaskObject
{
	private void Start()
	{
		base.Start();
		_taskObjectType = TaskObjectType.Ball;
	}
}