using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;

public class FreeCameraController : MonoBehaviour
{
    [SerializeField]
    private float _panSpeed = 15f;
    [SerializeField]
    private float _fastPanMultiplier = 2f;
    [SerializeField] 
    private float _zoomSpeed = 100f;
    [SerializeField]
    private float _minPositionY = 4f;
    [SerializeField]
    private float _maxPositionY = 12f;
    [SerializeField] 
    private float _rotationSpeed = 100f;
    [SerializeField]
    private float _minRotationX = 25f;
    [SerializeField]
    private float _maxRotationX = 45f;

    private bool _fastPanActive = false;

    private CameraInputActions _cameraInputActions;
    private InputAction _panAction;
    private InputAction _fastPanAction;
    private InputAction _zoomAction;
    private InputAction _rotateAction;

    private CinemachineVirtualCamera _virtualCamera;
    private Transform _cameraTransform;

    void Awake()
    {
        _cameraInputActions = new CameraInputActions();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (_virtualCamera == null)
        {
            Debug.LogError("'CameraControl' failed to find CinemachineVirtualCamera.");
            return;
        }
        _cameraTransform = _virtualCamera.VirtualCameraGameObject.transform;
    }

    private void OnEnable() 
    {
        _panAction = _cameraInputActions.CameraControls.Pan;
        _panAction.Enable();

        _fastPanAction = _cameraInputActions.CameraControls.FastPan;
        _fastPanAction.Enable();
        _fastPanAction.performed += ToggleFastPan;
        _fastPanAction.canceled += ToggleFastPan;

        _zoomAction = _cameraInputActions.CameraControls.Zoom;
        _zoomAction.Enable();
        
        _rotateAction = _cameraInputActions.CameraControls.Rotate;
        _rotateAction.Enable();
    }

    private void OnDisable()
    {
        _panAction.Disable();
        _fastPanAction.performed -= ToggleFastPan;
        _fastPanAction.Disable();
        _zoomAction.Disable();
        _rotateAction.Disable();
    }

    private void Update()
    {
        CheckPan();
        CheckZoom();
        CheckRotation();
    }

    private void CheckPan()
    {
        Vector2 panInput = _panAction.ReadValue<Vector2>();
        float panX = panInput.x;
        float panZ = panInput.y;

        if (panX == 0 && panZ == 0) return;

        PanCamera(panX, panZ);
    }

    private void CheckZoom()
    {
        // we might want to change this to zoom based on camera direction
        float zoomInput = _zoomAction.ReadValue<float>();

        if (zoomInput == 0) return;

        ZoomCamera(zoomInput);
    }

    private void CheckRotation()
    {
        var rotateInput = _rotateAction.ReadValue<Vector2>();

        if (rotateInput.x == 0) return;

        RotateCamera(rotateInput.x);
    }

    private void PanCamera(float x, float z)
    {
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        // ignore y values
        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 relativeMovement = cameraRight * x + cameraForward * z;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                                                 _cameraTransform.position + relativeMovement,
                                                 Time.deltaTime * _panSpeed * (_fastPanActive ? _fastPanMultiplier : 1));
    }

    private void ToggleFastPan(InputAction.CallbackContext context)
    {
        _fastPanActive = !_fastPanActive;
    }

    private void ZoomCamera(float increment)
    {
        float targetPositionY = Mathf.Clamp(_cameraTransform.position.y + increment, _minPositionY, _maxPositionY);
        float targetRotationX = Mathf.Clamp(_cameraTransform.eulerAngles.x + increment, _minRotationX, _maxRotationX);

        _cameraTransform.position = new Vector3(_cameraTransform.position.x,
                                                Mathf.Lerp(_cameraTransform.position.y, targetPositionY, _zoomSpeed * Time.deltaTime),
                                                _cameraTransform.position.z);

        _cameraTransform.eulerAngles = new Vector3(Mathf.Lerp(_cameraTransform.eulerAngles.x, targetRotationX, _rotationSpeed * Time.deltaTime),
                                                   _cameraTransform.eulerAngles.y,
                                                   _cameraTransform.eulerAngles.z);
    }

    private void RotateCamera(float rotateY)
    {
        Vector3 targetRotation = new Vector3(_cameraTransform.eulerAngles.x, _cameraTransform.eulerAngles.y + rotateY, _cameraTransform.eulerAngles.z);
        _cameraTransform.eulerAngles = Vector3.Lerp(_cameraTransform.eulerAngles, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
