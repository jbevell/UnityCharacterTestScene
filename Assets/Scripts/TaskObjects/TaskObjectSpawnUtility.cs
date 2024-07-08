using UnityEngine;

public static class TaskObjectSpawnUtility
{
	public static GameObject BreadstickPrefab;
	public static GameObject BallPrefab;

	public static void MakeBread(Vector3 position, Quaternion rotation)
	{
		Object.Instantiate(BreadstickPrefab, position, rotation);
	}

	public static void MakeBall(Vector3 position, Quaternion rotation)
	{
		Object.Instantiate(BallPrefab, position, rotation);
	}
}