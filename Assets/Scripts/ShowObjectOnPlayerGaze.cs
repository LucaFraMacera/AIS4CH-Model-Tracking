using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectOnPlayerGaze : MonoBehaviour
{
    [SerializeField]
    GameObject objectToHide;

    [Header("Settings")]
    [Range(0, 180)]
    public float viewAngle = 10f;

    GameObject _playerCamera;

    void Start()
    {
        _playerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerCamera == null || objectToHide == null) {
            return;
        } 
        Vector3 directionToObject = (objectToHide.transform.position - _playerCamera.transform.position).normalized;
        float dotProduct = Vector3.Dot(_playerCamera.transform.forward, directionToObject);
        float threshold = Mathf.Cos(viewAngle * Mathf.Deg2Rad);
        objectToHide.SetActive(dotProduct >= threshold);
        
    }

}
