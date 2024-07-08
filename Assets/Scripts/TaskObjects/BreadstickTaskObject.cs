using UnityEngine;

public class BreadstickTaskObject : TaskObject
{
	private new void Start()
    {
		base.Start();
		_taskObjectType = TaskObjectType.Breadstick;
    }
}
