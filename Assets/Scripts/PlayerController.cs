using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private float _acceleration = 0.1f;
    [SerializeField] private bool _enableControl;

    private int _currentRotation = 0;
    private float[] _rotations = new float[]
    {
        0, 45, 90, 45, 0, -45, -90, -45
    };

    public event Action onFall;
    private bool _onFallExecuted = false;

    public bool falling { get; private set; }
    public bool enableControl
    {
        get
        {
            return _enableControl;
        }

        set
        {
            if (falling)
                return;

            _enableControl = value;
        }
    }
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


    private void Update()
    {
        bool grounded = CheckGround();

        if (!grounded && !falling)
        {
            falling = true;
            return;
        }

        if (grounded && falling)
            falling = false;

        if (falling)
        {
            if (!_onFallExecuted)
            {
                onFall?.Invoke();
                _onFallExecuted = true;
            }

            _enableControl = false;

            Falling();
        }
        else if (_enableControl)
        {
            ComputeInput();
            ComputeRotation();
            ComputeMovement();
            IncreaseSpeed();
        }
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


    public void Restart(Vector3 restartPoint)
    {
        _currentRotation = 0;
        _onFallExecuted = false;

        falling = false;
        movementSpeed = 0;

        transform.position = restartPoint;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _rotations[0], transform.eulerAngles.z);
    }
}
