using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CameraMode
    {
        Follow,
        Free,
        Crafting,
    }

    public CameraMode activeCameraMode = CameraMode.Follow;
    public CameraMode previousNormalCameraMode = CameraMode.Follow;

    private CameraInputActions _cameraInputActions;

    [SerializeField]
    private GameObject _followCamera;
    [SerializeField]
    private GameObject _freeCamera;
    [SerializeField]
    private GameObject _craftingCamera;

    private void Awake()
    {
        _cameraInputActions = new CameraInputActions();

        EnableActiveCamera();
    }

    private void Start()
    {
        GameManager.instance.AddEnterAction(GameManager.GameMode.Crafting, () => SetCameraMode(CameraMode.Crafting));
        GameManager.instance.AddExitAction(GameManager.GameMode.Crafting, () => SetCameraMode(previousNormalCameraMode));
    }

    private void OnEnable()
    {
        _cameraInputActions.Enable();
        _cameraInputActions.CameraControls.ToggleCameraLock.performed += _ => ToggleCameraLock();
        _cameraInputActions.CameraControls.UnlockCamera.performed += _ => SetCameraMode(CameraMode.Free);
    }

    private void OnDisable()
    {
        _cameraInputActions.CameraControls.ToggleCameraLock.performed -= _ => ToggleCameraLock();
        _cameraInputActions.CameraControls.UnlockCamera.performed -= _ => SetCameraMode(CameraMode.Free);
        _cameraInputActions.Disable();
    }

    private void EnableActiveCamera()
    {
        _followCamera?.SetActive(activeCameraMode == CameraMode.Follow);
        _freeCamera?.SetActive(activeCameraMode == CameraMode.Free);
        _craftingCamera?.SetActive(activeCameraMode == CameraMode.Crafting);
    }

    private void ToggleCameraLock()
    {
        if (activeCameraMode == CameraMode.Free)
        {
            SetCameraMode(CameraMode.Follow);
        }
        else if (activeCameraMode == CameraMode.Follow)
        {
            SetCameraMode(CameraMode.Free);
        }
    }

    private void SetCameraMode(CameraMode cameraMode)
    {
        if (activeCameraMode == cameraMode) return;

        activeCameraMode = cameraMode;
        if (cameraMode == CameraMode.Follow || cameraMode == CameraMode.Free) previousNormalCameraMode = cameraMode;
        EnableActiveCamera();
    }
}