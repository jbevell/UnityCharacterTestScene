using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody PlayerRigidBody;
    
	private Vector3 _direction;
	private Vector3 _playerMovementInput;
	private bool _isMoving = false;
	private bool _isGrounded = false;
	private float _speed = 40;
	private float _velocityMagnitudeCap = 10;

	//private const float _turnSpeed = 10;
	private const float _rideHeight = 3;

	// Start is called before the first frame update
	void Start()
    {
		
	}

	private void Update()
	{
		_isMoving = false;
		_playerMovementInput = Vector3.zero;
		///Vector3 forward = transform.forward;

		if (Input.GetKey(KeyCode.W))
		{
			_playerMovementInput.z = _speed;
			_isMoving = true;
		}

		if (Input.GetKey(KeyCode.S))
		{
			_playerMovementInput.z = -_speed;
			_isMoving = true;
		}

		if (Input.GetKey(KeyCode.D))
		{
			_playerMovementInput.x = _speed;
			_isMoving = true;
		}

		if (Input.GetKey(KeyCode.A))
		{
			_playerMovementInput.x = -_speed;
			_isMoving = true;
		}
	}

	void FixedUpdate()
    {
		AdjustHeight();
		MoveCharacter();
		RotateCharacter();
	}

	// TODO: Inverse force seems to be applied to object below player. Magic ball effect. Stop that.
	private void AdjustHeight()
	{
		RaycastHit raycastInfo;
		Rigidbody hitRigidBody;

		Vector3 downwardRayDirection = -transform.up;
		Vector3 playerRigidBodyVelocity = PlayerRigidBody.velocity;
		Vector3 hitObjectVelocity = Vector3.zero;

		float rayDirectionMagnitude;
		float hitObjectDirectionMagnitude;

		float rideSpringStrength = 50f;
		float rideSpringDamper = 5f;

		Physics.Raycast(PlayerRigidBody.position, downwardRayDirection, out raycastInfo);

		hitRigidBody = raycastInfo.rigidbody;

		if (hitRigidBody != null)
		{
			hitObjectVelocity = hitRigidBody.velocity;
		}

		rayDirectionMagnitude = Vector3.Dot(downwardRayDirection, playerRigidBodyVelocity);
		hitObjectDirectionMagnitude = Vector3.Dot(downwardRayDirection, hitObjectVelocity);

		float relativeMagnitude = rayDirectionMagnitude - hitObjectDirectionMagnitude;

		float rideHeightDifference = raycastInfo.distance - _rideHeight;

		float springForce = (rideHeightDifference * rideSpringStrength) - (relativeMagnitude * rideSpringDamper);

		Vector3 characterCounterForce = downwardRayDirection * springForce;
		PlayerRigidBody.AddForce(characterCounterForce);

		if (hitRigidBody != null)
		{
			hitRigidBody.AddForceAtPosition(downwardRayDirection * -springForce, raycastInfo.point);
		}

		Debug.DrawLine(PlayerRigidBody.position, PlayerRigidBody.position + characterCounterForce, Color.cyan); // SpringForce
		Debug.DrawLine(raycastInfo.point, raycastInfo.point + (downwardRayDirection * -springForce), Color.magenta); // Raycast  Hit RigidBody Force
		Debug.DrawLine(PlayerRigidBody.position, raycastInfo.point, Color.green); // Raycast Debug
	}

    private void MoveCharacter()
    {
		Vector3 movePosition = transform.localPosition;

		PlayerRigidBody.AddForce(_playerMovementInput);
		
		Debug.DrawLine(PlayerRigidBody.position, PlayerRigidBody.position + PlayerRigidBody.velocity, Color.red); // Velocity Debug
		Debug.DrawLine(PlayerRigidBody.position, PlayerRigidBody.position + _playerMovementInput, Color.blue); // Force Debug

		if (!_isMoving && PlayerRigidBody.velocity.magnitude != 0)
		{
			PlayerRigidBody.drag = 8;

			PlayerRigidBody.angularVelocity.Normalize();
			PlayerRigidBody.angularVelocity = Vector3.Scale(PlayerRigidBody.angularVelocity, Vector3.zero);
		}
		else
			PlayerRigidBody.drag = 0;

		PlayerRigidBody.velocity = Vector3.ClampMagnitude(PlayerRigidBody.velocity, _velocityMagnitudeCap);
	}

	private void RotateCharacter()
	{
		if (Vector3.Scale(PlayerRigidBody.velocity, new Vector3(1, 0, 1)).magnitude > 0.1f)
			transform.rotation = Quaternion.LookRotation(new Vector3(PlayerRigidBody.velocity.x, 0, PlayerRigidBody.velocity.z));

		////CharacterRigidBody.AddTorque(new Vector3(0, 100, 0));
	}
}
