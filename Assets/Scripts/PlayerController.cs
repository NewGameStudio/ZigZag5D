using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private float _acceleration = 0.1f;

    private int _currentRotation = 0;
    private float[] _rotations = new float[]
    {
        0, 45, 90, 45, 0, -45, -90, -45
    };

    public float movementSpeed
    {
        get
        {
            return _movementSpeed;
        }

        set
        {
            _movementSpeed = Mathf.Clamp(value, 1, 3f);
        }
    }
    public bool isFreezed
    {
        get
        {
            return enabled;
        }
    }
    public bool isFalling { get; private set; }


    private void Update()
    {
        isFalling = isFalling || !CheckGround();

        if (isFalling)
        {
            Falling();
            return;
        }

        ComputeInput();
        ComputeRotation();
        ComputeMovement();
        IncreaseSpeed();
    }

    private bool CheckGround()
    {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), transform.lossyScale.x / 8);
    }

    private void ComputeInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        {
            _currentRotation++;

            if (_currentRotation >= _rotations.Length)
                _currentRotation = 0;
        }
    }

    private void ComputeRotation()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            Mathf.LerpAngle(transform.eulerAngles.y, _rotations[_currentRotation], _rotationSpeed * Time.deltaTime),
            transform.eulerAngles.z);
    }

    private void ComputeMovement()
    {
        transform.position += transform.forward * _movementSpeed * Time.deltaTime;
    }

    private void IncreaseSpeed()
    {
        movementSpeed = movementSpeed + _acceleration * Time.deltaTime;
    }

    private void Falling()
    {
        transform.position -= transform.up * 20 * Time.deltaTime;
    }


    public void Freeze()
    {
        enabled = false;
    }

    public void Unfreeze()
    {
        enabled = true;
    }

    public void Reset()
    {
        isFalling = false;

        _movementSpeed = 1;

        _currentRotation = 0;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _rotations[0], transform.eulerAngles.z);
    }
}
