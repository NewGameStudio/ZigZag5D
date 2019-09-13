using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool _followPlayer = true;
    public bool followPlayer
    {
        get
        {
            return _followPlayer;
        }

        set
        {
            _followPlayer = value;

            Update();
        }
    }

    [SerializeField]
    private Transform _player;
    private Vector3 _anchor;


    private void Awake()
    {
        if (_player == null)
        {
            Debug.Log("CameraController::player is null");
            Destroy(this);
            return;
        }

        _anchor = transform.position - _player.transform.position;
    }

    private void Update()
    {
        if (!_followPlayer)
            return;

        transform.position = _player.position + _anchor;
    }
}
