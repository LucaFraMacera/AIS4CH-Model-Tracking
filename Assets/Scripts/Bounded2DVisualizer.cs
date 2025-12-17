using Meta.XR.MRUtilityKit;
using Meta.XR.Samples;

using UnityEngine;

public class Bounded2DVisualizer : MonoBehaviour
    {
        [SerializeField]
        LineRenderer _lineRenderer;
        [SerializeField]
        RectTransform _canvasRect;
        [SerializeField, Tooltip("(in Canvas-local units)")]
        Vector3 _canvasOffset = new(0f, -15f, 0f);
        [SerializeField]
        GameObject qrCodeCenter;

        MRUKTrackable _trackable;

        Rect _box;

        GameObject _renderedModel;

        public void Initialize(MRUKTrackable trackable)
        {
            _trackable = trackable;

            if (trackable.PlaneBoundary2D == null && trackable.PlaneRect == null)
            {
                Debug.LogWarning($"{trackable} is missing a plane component.");
            }
            else
            {
                UpdateBoundingBox();
            }
        }

        void Update()
        {
            if (!_trackable || !_canvasRect)
            {
                return;
            }

            if(_trackable.IsTracked) {
                qrCodeCenter.SetActive(true);
            }

            UnityEngine.Assertions.Assert.IsTrue(_trackable.PlaneRect.HasValue);
            _box = _trackable.PlaneRect.Value;

            UpdateBoundingBox();
            UpdateModelPosition();
            UpdateCanvasPosition();
        
        }

        void UpdateBoundingBox()
        {
             if(_trackable.IsTracked) {  
                _lineRenderer.gameObject.SetActive(true);
                _lineRenderer.positionCount = 4;
                _lineRenderer.SetPosition(0, new Vector3(_box.x, _box.y, 0));
                _lineRenderer.SetPosition(1, new Vector3(_box.x + _box.width, _box.y, 0));
                _lineRenderer.SetPosition(2, new Vector3(_box.x + _box.width, _box.y + _box.height, 0));
                _lineRenderer.SetPosition(3, new Vector3(_box.x, _box.y + _box.height, 0));
            } else {
                _lineRenderer.gameObject.SetActive(false);
            }
            
        }

        void UpdateCanvasPosition(){
             if(_trackable.IsTracked) {  
                _canvasRect.gameObject.SetActive(true);
                _canvasRect.transform.position = _trackable.transform.position + _canvasOffset;
            }
        }

        void UpdateModelPosition() {
            if(!_renderedModel || !_trackable) {
                return;
            }
            if(_trackable.IsTracked && !_renderedModel.activeSelf) {  
                _renderedModel.SetActive(true);
            }
        }

        public void HideModel() {
            _renderedModel.SetActive(false);
            _lineRenderer.gameObject.SetActive(false);
            _canvasRect.gameObject.SetActive(false);
            qrCodeCenter.SetActive(false);
        }

        public void SetModel(GameObject model) {
            _renderedModel = model;
        }

    }
