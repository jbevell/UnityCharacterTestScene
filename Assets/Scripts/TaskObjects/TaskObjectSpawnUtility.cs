using UnityEngine;

public class TaskObjectSpawnUtility : MonoBehaviour
{
	public GameObject BreadstickPrefab;
	public GameObject BallPrefab;

	public void MakeBread(Vector3 position, Quaternion rotation)
	{
		Instantiate(BreadstickPrefab, position, rotation);
	}

	public void MakeBall(Vector3 position, Quaternion rotation)
	{
		Instantiate(BallPrefab, position, rotation);
	}
}