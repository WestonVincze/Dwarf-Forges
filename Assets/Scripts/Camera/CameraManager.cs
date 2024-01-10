using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // disable while in free camera mode
    _cameraInputActions.CameraControls.UnlockCamera.performed += ResetFreeCamera;
  }

  private void OnDisable()
  {
    _cameraInputActions.CameraControls.UnlockCamera.performed -= ResetFreeCamera;
    _cameraInputActions.Disable();
  }

  private void EnableActiveCamera()
  {
    _followCamera?.SetActive(activeCameraMode == CameraMode.Follow);
    _freeCamera?.SetActive(activeCameraMode == CameraMode.Free);
    _craftingCamera?.SetActive(activeCameraMode == CameraMode.Crafting);
  }

  private void ResetFreeCamera(InputAction.CallbackContext context)
  {
    _freeCamera.transform.position = _followCamera.transform.position;
    _freeCamera.transform.rotation = _followCamera.transform.rotation;
    SetCameraMode(CameraMode.Free);
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

    if (cameraMode == CameraMode.Follow)
    {
      _cameraInputActions.CameraControls.UnlockCamera.Enable();
    }
    else
    {
      _cameraInputActions.CameraControls.UnlockCamera.Disable();
    }

    activeCameraMode = cameraMode;
    if (cameraMode == CameraMode.Follow || cameraMode == CameraMode.Free) previousNormalCameraMode = cameraMode;
    EnableActiveCamera();
  }
}