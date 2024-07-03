using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRotator : MonoBehaviour
{
    private Rigidbody _playerRigidBody;

    private bool _constantRotation = false;
    private const float _constantRotationSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        _playerRigidBody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_constantRotation)
        {
            transform.Rotate(_constantRotationSpeed, 0, 0);
            return;
        }
        
        if (_playerRigidBody != null)
        {
            transform.Rotate(_playerRigidBody.velocity.magnitude, 0, 0);
        }
    }
}
