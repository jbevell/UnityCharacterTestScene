using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody PlayerRigidBody;
	public InputActionAsset GamepadInput;
	[SerializeField] private SpringJoint PlayerOrientationSpring;

	private UprightSpringBehaviour _springBehaviour;
	private Vector3 _playerMovementInput;
	private bool _isMoving = false;
	private bool _isGrounded = true;
	private bool _jumpPressed = false;
	private float _movementForce = 100;
	private float _velocityMagnitudeCap = 10;

	private Quaternion _playerTargetOrientation;

	[SerializeField] private bool _doTheFunkySpin = true;
	private readonly Vector3 funkySpinSpeed = new Vector3(1f, 1f, 1f);

	private const float _fallPower = 5f;
	private const float _lowJumpPower = 6f;
	private const float _rideHeight = 1.5f;
	private const float _rideSpringStrength = 1500f;
	private const float _rideSpringDamper = 25f;
	private const float _jumpPower = 60f;
	
	private void Start()
	{

		_springBehaviour = PlayerOrientationSpring.GetComponent<UprightSpringBehaviour>();

		if (_doTheFunkySpin)
		{
			PlayerRigidBody.useGravity = false;
			PlayerRigidBody.constraints = RigidbodyConstraints.None;
		}
		else
		{
			PlayerRigidBody.maxAngularVelocity = float.MaxValue;
			PlayerRigidBody.angularDrag = 5;
		}
	}

	private void Update()
	{
		_isMoving = false;
		_playerMovementInput = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			_playerMovementInput.z = _movementForce;
			_isMoving = true;
		}

		////GamepadInput.FindActionMap("Schmove");

		if (Input.GetKey(KeyCode.S))
		{
			_playerMovementInput.z = -_movementForce;
			_isMoving = true;
		}

		if (Input.GetKey(KeyCode.D))
		{
			_playerMovementInput.x = _movementForce;
			_isMoving = true;
		}

		if (Input.GetKey(KeyCode.A))
		{
			_playerMovementInput.x = -_movementForce;
			_isMoving = true;
		}

		if (_playerMovementInput.magnitude > 0)
		{
			_springBehaviour.LookAtPlayerForce(Vector3.Normalize(_playerMovementInput));
			_playerTargetOrientation = PlayerOrientationSpring.transform.rotation;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_jumpPressed = true;
		}
	}

	void FixedUpdate()
	{
		AdjustHeight();
		MovePlayer();

		if (!_doTheFunkySpin)
		{
			AdjustRotation();
			//RotateCharacter();
			CounterCharacterRotation();
		}

		Jump();
		AdjustFallingAndJumping();

		if (_doTheFunkySpin)
			PlayerRigidBody.AddTorque(Vector3.Scale(UnityEngine.Random.onUnitSphere, funkySpinSpeed));

		////Debug.Log(_isGrounded);
		////Debug.Log(PlayerRigidBody.velocity.magnitude);
		////Debug.Log($"Time: {Time.time}");
	}

	private void AdjustHeight()
	{
		Vector3 downwardRayDirection = Vector3.down;
		RaycastHit downRaycastInfo;
		float rideHeightDifference;

		Physics.Raycast(PlayerRigidBody.position, downwardRayDirection, out downRaycastInfo);
		rideHeightDifference = downRaycastInfo.distance - _rideHeight;

		_isGrounded = rideHeightDifference <= 0.3f;

		// Jumping? Falling? No spring until grounded.
		if (!_isGrounded)
			return;

		Rigidbody hitRigidBody;

		Vector3 playerRigidBodyVelocity = PlayerRigidBody.velocity;
		Vector3 hitObjectVelocity = Vector3.zero;

		float rayDirectionMagnitude;
		float hitObjectDirectionMagnitude;

		float relativeMagnitude;
		float springForce;
		Vector3 characterCounterForce;

		////Debug.Log(downRaycastInfo.distance - _rideHeight);

		hitRigidBody = downRaycastInfo.rigidbody;

		if (hitRigidBody != null)
		{
			hitObjectVelocity = hitRigidBody.velocity;
			////Debug.Log($"Hit body velocity {hitObjectVelocity}");
		}

		rayDirectionMagnitude = Vector3.Dot(downwardRayDirection, playerRigidBodyVelocity);
		hitObjectDirectionMagnitude = Vector3.Dot(downwardRayDirection, hitObjectVelocity);

		relativeMagnitude = rayDirectionMagnitude - hitObjectDirectionMagnitude;
		springForce = (rideHeightDifference * _rideSpringStrength) - (relativeMagnitude * _rideSpringDamper);
		characterCounterForce = downwardRayDirection * springForce;
		
		if (!_doTheFunkySpin)
			PlayerRigidBody.AddForce(characterCounterForce);

		if (hitRigidBody != null && !_doTheFunkySpin)
		{
			hitRigidBody.AddForceAtPosition(downwardRayDirection * -springForce, downRaycastInfo.point);
		}

		////Debug.Log(characterCounterForce);

		Debug.DrawLine(PlayerRigidBody.position, PlayerRigidBody.position + characterCounterForce, Color.cyan); // SpringForce
		////Debug.DrawLine(downRaycastInfo.point, downRaycastInfo.point + (downwardRayDirection * -springForce), Color.magenta); // Raycast  Hit RigidBody Force
		////Debug.DrawLine(PlayerRigidBody.position, downRaycastInfo.point, Color.green); // Raycast Debug
	}

	private void AdjustRotation()
	{
		Quaternion currentPlayerRotation = transform.rotation;
		Quaternion rotationTarget = QuaternionUtility.ShortestRotation(_playerTargetOrientation, currentPlayerRotation);
		
		float rotDegrees;
		Vector3 rotAxis;

		Debug.Log($"PlayerTargetOrientation is currently {QuaternionUtility.FormatAsQuaternionExpression(_playerTargetOrientation)}");
		Debug.Log($"RotationTarget is {QuaternionUtility.FormatAsQuaternionExpression(rotationTarget)}");

		rotationTarget.ToAngleAxis(out rotDegrees, out rotAxis);
		rotationTarget.Normalize();

		float rotRadians = rotDegrees * Mathf.Deg2Rad;

		Vector3 torqueValue = (rotAxis * (rotRadians * PlayerOrientationSpring.spring)) - (PlayerRigidBody.angularVelocity * PlayerOrientationSpring.damper);

		Debug.Log($"Torque value is {torqueValue}");

		PlayerRigidBody.AddTorque(torqueValue);
	}

	private void MovePlayer()
    {
		Vector3 movePosition = transform.localPosition;

		if (_isGrounded)
		{
			PlayerRigidBody.velocity = Vector3.ClampMagnitude(PlayerRigidBody.velocity, _velocityMagnitudeCap);
		}
		else
		{
			Vector3 tempVelocity = PlayerRigidBody.velocity;
			tempVelocity = Vector3.Scale(tempVelocity, new Vector3(1, 0, 1));
			tempVelocity = Vector3.ClampMagnitude(tempVelocity, _velocityMagnitudeCap);

			PlayerRigidBody.velocity = new Vector3(tempVelocity.x, PlayerRigidBody.velocity.y, tempVelocity.z);
		}

		PlayerRigidBody.velocity = Vector3.ClampMagnitude(PlayerRigidBody.velocity, _velocityMagnitudeCap);

		PlayerRigidBody.AddForce(_playerMovementInput);

		Debug.DrawLine(PlayerRigidBody.position, PlayerRigidBody.position + PlayerRigidBody.velocity, Color.red); // Velocity Debug
		Debug.DrawLine(PlayerRigidBody.position, PlayerRigidBody.position + _playerMovementInput, Color.blue); // Force Debug

		if (!_isMoving && _isGrounded && PlayerRigidBody.velocity.magnitude != 0)
		{
			PlayerRigidBody.drag = 8;

			PlayerRigidBody.angularVelocity.Normalize();
			PlayerRigidBody.angularVelocity = Vector3.Scale(PlayerRigidBody.angularVelocity, Vector3.zero);
		}
		else
			PlayerRigidBody.drag = 0;
	}

	// TODO: Attempting to replace with torque forces. Calculate proper torque value and values to slow the rotation down so that player stops when facing velocity vector.
	// Would a PD controller work for this? Could calculate the point on the x,z plane and just always have that point be the forward vector of the player.
	private void RotateCharacter()
	{
		bool useTorque = false;

		if (useTorque)
		{
			Vector3 adjustedVelocity = new Vector3(PlayerRigidBody.velocity.x, 0, PlayerRigidBody.velocity.z);

			Vector3 turnTorque = Vector3.Cross(adjustedVelocity, new Vector3(1, 0, 1));

			PlayerRigidBody.AddTorque(turnTorque);

			Debug.Log(turnTorque);
			Debug.DrawLine(PlayerRigidBody.transform.position, PlayerRigidBody.transform.position + turnTorque, Color.magenta);
		}
		else
		{
			// Original turn handling
			Vector3 lookVelocity = Vector3.Scale(PlayerRigidBody.velocity, new Vector3(1, 0, 1));

			if (lookVelocity.magnitude > 0.1f && !_doTheFunkySpin)
				transform.rotation = Quaternion.LookRotation(lookVelocity);
		}
	}

	private void CounterCharacterRotation()
	{
		////Vector3 adjustedVelocity = new Vector3(PlayerRigidBody.velocity.x, 0, PlayerRigidBody.velocity.z);
		////Vector3 turnTorque = Vector3.Cross(adjustedVelocity, new Vector3(1, 0, 1));

		////float yTorque = turnTorque.y * 0.5f;

		////Vector3 rotationalVelocity = PlayerRigidBody.angularVelocity;

		////yTorque -= rotationalVelocity.y * 1;

		////Vector3 counterTorque = new Vector3(0, yTorque, 0);

		////PlayerRigidBody.AddTorque(counterTorque);
		
		////Debug.Log(yTorque);
		////Debug.Log("Angular rotation " + rotationalVelocity);
		////Debug.DrawLine(PlayerRigidBody.transform.position, PlayerRigidBody.transform.position + counterTorque, Color.green);
	}

	private void Jump()
	{
		if (!_jumpPressed)
			return;

		_jumpPressed = false;

		if (!_isGrounded)
			return;

		Debug.Log("Jumping!");
		
		Vector3 jumpForce = Vector3.up * _jumpPower;
		Vector3 directionalJumpForce = PlayerRigidBody.velocity + jumpForce;

		PlayerRigidBody.AddForce(directionalJumpForce, ForceMode.Impulse);

		////UnityEngine.Debug.DrawLine(PlayerRigidBody.position, PlayerRigidBody.position + directionalJumpForce, Color.black);
	}

	private void AdjustFallingAndJumping()
	{
		if (!_isGrounded && !_doTheFunkySpin)
		{
			if (PlayerRigidBody.velocity.y < 0)
				PlayerRigidBody.velocity += Vector3.up * Physics.gravity.y * _fallPower * Time.fixedDeltaTime;
			else if (PlayerRigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
				PlayerRigidBody.velocity += Vector3.up * Physics.gravity.y * _lowJumpPower * Time.fixedDeltaTime;
		}
	}
}
