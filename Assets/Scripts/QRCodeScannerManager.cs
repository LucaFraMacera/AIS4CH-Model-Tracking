using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Meta.XR.MRUtilityKitSamples.QRCodeDetection;
using System;
using UnityEngine;
using System.Threading.Tasks;

public class QRCodeScannerManager : MonoBehaviour
{
        [SerializeField]
        MRUK _mrukInstance;

        [SerializeField]
        MyQRCodeHandler qrCodeHandler;

        static QRCodeScannerManager s_instance;

        void OnValidate()
        {
            if (!_mrukInstance && FindAnyObjectByType<MRUK>() is { } mruk && mruk.gameObject.scene == gameObject.scene)
            {
                _mrukInstance = mruk;
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
            StartCoroutine(ScanCode(trackable));
        }

        public void OnTrackableRemoved(MRUKTrackable trackable)
        {
            if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            {
                return;
            }
            Destroy(trackable.gameObject);
        }

        private IEnumerator ScanCode(MRUKTrackable trackable) {
            if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode){
                Debug.Log("QR Code is not scannable");
                yield break;
            }
            string payload = trackable.MarkerPayloadString;
            GameObject container = new GameObject(payload);
            container.transform.position = trackable.transform.position;
            string url = $"http://192.168.0.138:8080/api/models/{payload}";
            Debug.LogWarning($"Requesting model at {url}");
            long startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Task<Model> requestModelTask = Client.Get<Model>(url);
            MyQRCode qrCodeInstance = this.qrCodeHandler.Initialize(trackable);
            yield return  new WaitUntil(()=>requestModelTask.IsCompleted);
            Model model = requestModelTask.Result;
            qrCodeInstance.SetModel(model);
            long requestTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - startTime;
            Debug.LogWarning($"Model found. Request time: {requestTime} seconds");
            if(model == null) {
                yield break;
            }
            yield return StartCoroutine(ModelSpawnManager.Instance.RenderModel(model, container));
            Bounded2DVisualizer qrCodeVisualizer = qrCodeInstance.GetComponent<Bounded2DVisualizer>();
            qrCodeVisualizer.SetModel(container.transform.parent.gameObject);
        }


}
