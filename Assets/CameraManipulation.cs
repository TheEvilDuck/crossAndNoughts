using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulation : MonoBehaviour
{  
    [SerializeField]private float _maxMagnitude = 2f;
    [SerializeField]private float _minFingerSlideMagnitudeToMoveCamera = 1f;
    [SerializeField]private float _maxZoom = 2f;
    private Vector3 _touchStart;
    private  Camera _camera;
    private Transform _cameraTransform;
    private Vector3 _startCameraPosition;
    private float _startCameraSize;
    private bool _cameraManipulating;

    private void Start() 
    {
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
        _startCameraPosition = _cameraTransform.position;
        _startCameraSize = _camera.orthographicSize;
    }
    public bool GetCameraManipulating()
    {
        return _cameraManipulating;
    }
    public void ResetCamera()
    {
        _camera.orthographicSize = _startCameraSize;
        _cameraTransform.position = _startCameraPosition;
    }
    private void Update() 
    {
        _cameraManipulating = false;
        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount==2)
        {
            var touch1 = Input.touches[0];
            var touch2 = Input.touches[1];
            Vector2 prevPos1 = touch1.position-touch1.deltaPosition;
            Vector2 prevPos2 = touch2.position-touch2.deltaPosition;

            float prevMagnitude = (prevPos1-prevPos2).magnitude;
            float currentMagnitude = (touch1.position-touch2.position).magnitude;

            float zoomDelta = currentMagnitude-prevMagnitude;

            float zoomedSize = _camera.orthographicSize-zoomDelta*0.01f;
            if (zoomedSize>=_startCameraSize/_maxZoom
            &&zoomedSize<=_startCameraSize)
                {
                    _camera.orthographicSize=zoomedSize;
                    _cameraManipulating = true;
                }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = _touchStart - _camera.ScreenToWorldPoint(Input.mousePosition);
            if ((_startCameraPosition-(_cameraTransform.position+direction)).magnitude<=_maxMagnitude
                &&direction.magnitude>=_minFingerSlideMagnitudeToMoveCamera)
            {
                _cameraTransform.Translate(direction);
                _cameraManipulating = true;
            }
            
        }
    }
}
