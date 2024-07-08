public class BallTaskObject : TaskObject
{
	private new void Start()
	{
		base.Start();
		_taskObjectType = TaskObjectType.Ball;
	}
}