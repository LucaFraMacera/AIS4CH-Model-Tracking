using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Meta.XR.MRUtilityKitSamples.QRCodeDetection;
using System;
using UnityEngine;

public class QRManager : MonoBehaviour
{

//
        // Static interface

        public const string ScenePermission = OVRPermissionsRequester.ScenePermission;

        public static bool IsSupported
            => OVRAnchor.TrackerConfiguration.QRCodeTrackingSupported;

        public static bool HasPermissions
        #if UNITY_EDITOR
                    => true;
        #else
                    => UnityEngine.Android.Permission.HasUserAuthorizedPermission(ScenePermission);
        #endif

        public static bool TrackingEnabled
        {
            get => s_instance && s_instance._mrukInstance && s_instance._mrukInstance.SceneSettings.TrackerConfiguration.QRCodeTrackingEnabled;
            set
            {
                if (!s_instance || !s_instance._mrukInstance)
                {
                    return;
                }
                var config = s_instance._mrukInstance.SceneSettings.TrackerConfiguration;
                config.QRCodeTrackingEnabled = value;
                s_instance._mrukInstance.SceneSettings.TrackerConfiguration = config;
            }
        }


        public static void RequestRequiredPermissions(Action<bool> onRequestComplete)
        {
            if (!s_instance)
            {
                Debug.LogError($"{nameof(RequestRequiredPermissions)} failed; no QRCodeManager instance.");
                return;
            }

            #if UNITY_EDITOR
                        const string kCantRequestMsg =
                            "Cannot request Android permission when using Link or XR Sim. " +
                            "For Link, enable the spatial data permission from the Link app under Settings > Beta > Spatial Data over Meta Quest Link. " +
                            "For XR Sim, no permission is necessary.";


                        onRequestComplete?.Invoke(HasPermissions);
            #else

                        var callbacks = new UnityEngine.Android.PermissionCallbacks();
                        callbacks.PermissionGranted += perm => Log($"{perm} granted");

                        var msgDenied = $"{ScenePermission} denied. Please press the 'Request Permission' button again.";
                        var msgDeniedPermanently = $"{ScenePermission} permanently denied. To enable:\n" +
                                                $"    1. Uninstall and reinstall the app, OR\n" +
                                                $"    2. Manually grant permission in device Settings > Privacy & Safety > App Permissions.";

            #if !UNITY_6000_0_OR_NEWER
                        callbacks.PermissionDenied += _ => Log(msgDenied, LogType.Error);
                        callbacks.PermissionDeniedAndDontAskAgain += _ => Log(msgDeniedPermanently, LogType.Error);
            #else
                        callbacks.PermissionDenied += perm =>
                        {
                            // ShouldShowRequestPermissionRationale returns false only if
                            // the user selected 'Never ask again' or if the user has never
                            // been asked for the permission (which can't be the case here).
                        };
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


        [SerializeField]
        MRUK _mrukInstance;

        [SerializeField]
        MyQRCodeHandler qrCodeHandler;

        static QRManager s_instance;

        static ModelSpawnManager modelManager;


        //
        // MonoBehaviour messages

        void OnValidate()
        {
            if (!_mrukInstance && FindAnyObjectByType<MRUK>() is { } mruk && mruk.gameObject.scene == gameObject.scene)
            {
                _mrukInstance = mruk;
            }

            if (!modelManager && FindAnyObjectByType<ModelSpawnManager>() is { } msm && msm.gameObject.scene == gameObject.scene)
            {
                modelManager = msm;
            }

        }

        void OnEnable()
        {
            s_instance = this;

            if (!_mrukInstance)
            {
                return;
            }

            _mrukInstance.SceneSettings.TrackableAdded.AddListener(OnTrackableAdded);
            _mrukInstance.SceneSettings.TrackableRemoved.AddListener(OnTrackableRemoved);
        }

        void OnDestroy()
            => s_instance = null;


        //
        // UnityEvent listeners

        public void OnTrackableAdded(MRUKTrackable trackable)
        {
            if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            {
                return;
            }

            var log = $"{nameof(OnTrackableAdded)}: QRCode tracked!\nUUID={trackable.Anchor.Uuid}";
            string payload = trackable.MarkerPayloadString;
            if (!modelManager) {
                return;
            }
            GameObject container = new GameObject("AA");
            container.transform.position = trackable.transform.position;
            qrCodeHandler.Initialize(trackable, container);
            modelManager.Spawn(payload, container);
        }

        public void OnTrackableRemoved(MRUKTrackable trackable)
        {
            if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            {
                return;
            }
            Destroy(trackable.gameObject);
        }


}
