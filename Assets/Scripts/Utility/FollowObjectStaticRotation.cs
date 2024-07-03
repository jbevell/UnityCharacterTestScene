using UnityEngine;

public class FollowObjectStaticRotation : MonoBehaviour
{
	[SerializeField] private GameObject _objectToFollow;
	[SerializeField] private float _maxSpeed = 10;

	private Vector3 offset;
	
	void Start()
	{
		offset = transform.position - _objectToFollow.transform.position;
	}

	void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, _objectToFollow.transform.position + offset, _maxSpeed);
	}
}
