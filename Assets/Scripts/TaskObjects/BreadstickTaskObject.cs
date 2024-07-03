using UnityEngine;

public class BreadstickTaskObject : TaskObject
{
	private void Start()
    {
		base.Start();
		_taskObjectType = TaskObjectType.Breadstick;
    }
}
