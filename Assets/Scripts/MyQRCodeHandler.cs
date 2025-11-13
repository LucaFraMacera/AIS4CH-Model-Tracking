using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Meta.XR.MRUtilityKitSamples.QRCodeDetection;
using System;
using UnityEngine;

public class MyQRCodeHandler : MonoBehaviour
{
    [SerializeField]
    MyQRCode _qrCodePrefab;

    public void Initialize(MRUKTrackable trackable, GameObject model) {
        var instance = Instantiate(_qrCodePrefab, trackable.transform);
        var qrCode = instance.GetComponent<MyQRCode>();
        qrCode.Initialize(trackable);
        instance.GetComponent<Bounded2DVisualizer>().Initialize(trackable, model);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
