using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Units.Player
{
    public class CameraComponent : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 25f)] private float _moveSpeed = 2f;
        [SerializeField, Range(1f, 100f)] private float _rotateSpeed = 2f;
        [SerializeField, Range(-90f, 0f), Tooltip("минимальный наклон Y")]
        private float _minY = -45f;
        [SerializeField, Range(0f, 90f), Tooltip("максимальный наклон Y")]
        private float _maxY = 30f;
        [SerializeField] private bool _inverseRotation;
        [SerializeField] private float _smoothing;
        [SerializeField, Range(0.1f, 15f)] private float _lockCameraSpeed;
        private PlayerControls _controls;
        private Unit _target;
        private Transform _pivot;
        private Transform _camera;
        private Vector3 _pivotEulers;
        //текущее положение вокруг осей 0X, 0Y
        private Vector2 _angles;
        private int _inverse;
        //поворот пивота по вертикали
        private Quaternion _pivotTargetRotation;
        //поворот точки по горизонтали
        private Quaternion _transformTargetRotation;
        private Quaternion _defaultCameraRotation;
        private void Awake()
        {
            _controls = new PlayerControls();
        }
        private void OnEnable()
        {
            _controls.Camera.Enable();
        }
        private void Start()
        {
            _target = transform.parent.GetComponent<Unit>();
            _pivot = transform.GetChild(0);
            _camera = GetComponentInChildren<Camera>().transform;
            _defaultCameraRotation = _camera.localRotation;
            transform.parent = null;
            _pivotEulers = _pivot.eulerAngles;
            _target.OnTargetLostEvent += () => _camera.localRotation = _defaultCameraRotation;
            if (_inverseRotation)
            {
                _inverse = 1;
            }
            else
            {
                _inverse = -1;
            }
        }
        private void LateUpdate()
        {
            transform.position = 
                Vector3.Lerp(transform.position, _target.transform.position, Time.deltaTime * _moveSpeed);
            if (_target.Target == null)
            {
                FreeCamera();
            }
            else
            {
                LockCamera();
            }
        }
        private void FreeCamera()
        {
            var delta = _controls.Camera.Delta.ReadValue<Vector2>() * Time.deltaTime;
            delta.y *= _inverse;
            _angles += delta * _rotateSpeed;
            _angles.y = Mathf.Clamp( _angles.y, _minY, _maxY);
            _pivotTargetRotation = Quaternion.Euler(_angles.y, _pivotEulers.y, _pivotEulers.z);
            _transformTargetRotation = Quaternion.Euler(0f, _angles.x, 0f);
            _pivot.localRotation = Quaternion.Slerp(_pivot.localRotation, _pivotTargetRotation, _smoothing * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation,  _transformTargetRotation, _smoothing * Time.deltaTime);
        }
        private void LockCamera()
        {
            var rotation = Quaternion.LookRotation(_target.Target.FocusPoint.position - _camera.position); ;
            _camera.rotation = Quaternion.Slerp(_camera.rotation, rotation, _lockCameraSpeed * Time.deltaTime);
            rotation = Quaternion.LookRotation(_target.Target.FocusPoint.position - _pivot.position);
            _pivot.rotation = Quaternion.Slerp(_pivot.rotation, rotation, _lockCameraSpeed * Time.deltaTime);

            //_camera.LookAt(_target.Target.transform.position, Vector3.up);
            //_pivot.LookAt(_target.Target.transform.position, Vector3.up);
        }
        private void OnDisable()
        {
            _controls.Camera.Disable();
            _target.OnTargetLostEvent -= () => _camera.localRotation = _defaultCameraRotation;
        }
        private void OnDestroy()
        {
            _controls.Dispose();
        }
    }
}
