using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool _isPaused;

    // TODO: default mode should be main menu
    private GameMode _currentGameMode = GameMode.LockedCamera;
    public GameMode currentGameMode
    {
        get { return _currentGameMode; }
    }

    public GameMode defaultGameMode = GameMode.LockedCamera;
    public enum GameMode
    {
        MainMenu,       // main menu UI is displayed
        Tutorial,       // before the first dwarf is spawned
        LockedCamera,   // regular gameplay with free camera control 
        FreeCamera,     // regular gameplay with locked camera on forge
        Crafting,       // crafting mode enabled with locked camera on crafting table
        StatReview,     // after a weapon is forged the game is paused and weapon stats are displayed to player
        PauseMenu,      // game is paused with pause menu UI displayed
    }

    // Crafting mode variables
    [SerializeField]
    private GameObject _craftingCamera; // GameObject because we only need to enable/disable

    // LockedCamera mode variables
    [SerializeField]
    private GameObject _lockedCamera; // GameObject because we only need to enable/disable

    // FreeCamera mode variables
    [SerializeField]
    private GameObject _freeCamera; // GameObject because we only need to enable/disable

    private void Awake()
    {
        // not the best, but it should be fine for now
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SetGameMode(GameMode mode)
    {
        // placeholder switch statement
        switch (mode)
        {
            case GameMode.Tutorial:
                ResumeGame();
                break;
            case GameMode.LockedCamera:
                ResumeGame();
                break;
            case GameMode.FreeCamera:
                ResumeGame();
                break;
            case GameMode.Crafting:
                ResumeGame();
                break;
            case GameMode.StatReview:
                PauseGame();
                break;
            case GameMode.PauseMenu:
                // TODO: display UI
                PauseGame();
                break;
        }

        // TODO: camera manager?
        _lockedCamera.SetActive(mode == GameMode.LockedCamera);
        _freeCamera.SetActive(mode == GameMode.FreeCamera);
        _craftingCamera.SetActive(mode == GameMode.Crafting);

        _currentGameMode = mode;
    }

    public void ToggleGameMode(GameMode mode)
    {
        if (_currentGameMode == mode)
        {
            SetGameMode(defaultGameMode);
        }
        else
        {
            SetGameMode(mode);
        }
    }

    public void ToggleCameraLock()
    {
        if (_currentGameMode == GameMode.LockedCamera)
        {
            SetGameMode(GameMode.FreeCamera);
        }
        else if (_currentGameMode == GameMode.FreeCamera)
        {
            SetGameMode(GameMode.LockedCamera);
        }
    }

    public void PauseGame()
    {
        if (_isPaused) return;
        _isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!_isPaused) return;
        _isPaused = false;
        Time.timeScale = 1f;
    }
}
