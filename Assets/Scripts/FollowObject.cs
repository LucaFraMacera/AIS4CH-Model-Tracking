using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowObject : MonoBehaviour
{

    [SerializeField]
    GameObject objectToFollow;

    [SerializeField]
    Vector3 offset;

    Vector3 _lastPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(objectToFollow.transform.position == Vector3.zero) {
            return;
        }
        Debug.LogWarning(OVRManager.hasInputFocus);
        gameObject.transform.position = objectToFollow.transform.position + offset;
    }

}
