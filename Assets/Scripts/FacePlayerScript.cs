using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerScript : MonoBehaviour
{
    GameObject _playerCamera;

    void Start()
    {
        _playerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    void Update()
    {
        if (_playerCamera)
        {
            transform.rotation =
                Quaternion.LookRotation(transform.position - _playerCamera.transform.position);
        }
    }
}
