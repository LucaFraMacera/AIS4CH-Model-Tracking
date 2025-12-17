using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Meta.XR.MRUtilityKitSamples.QRCodeDetection;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetPermissionsScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text resultText;

    public const string ScenePermission = OVRPermissionsRequester.ScenePermission;

    public static bool IsSupported
        => OVRAnchor.TrackerConfiguration.QRCodeTrackingSupported;

    public static bool HasPermissions
    #if UNITY_EDITOR
                => true;
    #else
                => UnityEngine.Android.Permission.HasUserAuthorizedPermission(ScenePermission);
    #endif

    public static string IsInterrupted = "";


    public static void RequestRequiredPermissions(Action<bool> onRequestComplete)
    {
        
        #if UNITY_EDITOR
            onRequestComplete?.Invoke(HasPermissions);
        #else
        var callbacks = new UnityEngine.Android.PermissionCallbacks();
            #if !UNITY_6000_0_OR_NEWER
                        callbacks.PermissionDenied += _ => Debug.LogError("Permission Denied");
                        callbacks.PermissionDeniedAndDontAskAgain += _ => Debug.LogError("Permission Denied Permanently");
            #else
                        callbacks.PermissionDenied += perm =>{ };
            #endif // UNITY_6000_0_OR_NEWER

        if (onRequestComplete is not null)
        {
            callbacks.PermissionGranted += _ => onRequestComplete(HasPermissions);
            callbacks.PermissionDenied += _ => onRequestComplete(HasPermissions);
            #if !UNITY_6000_0_OR_NEWER
                callbacks.PermissionDeniedAndDontAskAgain += _ => onRequestComplete(HasPermissions);
             #endif // UNITY_6000_0_OR_NEWER
        }

        UnityEngine.Android.Permission.RequestUserPermission(ScenePermission, callbacks);
        #endif // UNITY_EDITOR
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnValidate(){
        if(HasPermissions && IsSupported) {
            resultText.text = "";
            gameObject.SetActive(false);
        } else {
            resultText.text = HasPermissions ? "Abilita il tracciamento dei QR code nei permessi del tuo dispositivo." : "Il dispositivo non è abilitato a tracciare l'ambiente";
        }
    }

    public void RequestPermissions() {
        GetPermissionsScript.RequestRequiredPermissions(hasPerms =>
        {
            if(hasPerms) {
                gameObject.SetActive(false);
            } else {
                resultText.text = "Permessi mancanti. Riavvia l'app o chiedi all'amministratore.";
            }
        });
    }

}
