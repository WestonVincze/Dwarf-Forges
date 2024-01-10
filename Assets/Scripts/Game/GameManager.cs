using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool _isPaused;

    // TODO: default mode should be main menu
    private GameMode _currentGameMode = GameMode.Normal;
    [SerializeField] private bool _inDebugMode = false;
    public GameMode currentGameMode
    {
        get { return _currentGameMode; }
    }
    public bool inDebugMode
    {
        get => _inDebugMode;
    }

    public GameMode defaultGameMode = GameMode.Normal;
    public enum GameMode
    {
        MainMenu,       // main menu UI is displayed
        Tutorial,       // before the first dwarf is spawned
        Normal,         // normal gameplay
        Crafting,       // crafting mode enabled with locked camera on crafting table
        StatReview,     // after a weapon is forged the game is paused and weapon stats are displayed to player
        PauseMenu,      // game is paused with pause menu UI displayed
    }

    public Dictionary<GameMode, Action> enterActions = new Dictionary<GameMode, Action>();
    public Dictionary<GameMode, Action> exitActions = new Dictionary<GameMode, Action>();

    private void OnEnable()
    {
        enterActions.Add(GameMode.PauseMenu, PauseGame);
        exitActions.Add(GameMode.PauseMenu, ResumeGame);
    }

    private void OnDisable()
    {
        enterActions.Clear();
        exitActions.Clear();
    }

    private void Awake()
    {
        // not the best, but it should be fine for now
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddEnterAction(GameMode mode, Action action)
    {
        if (enterActions.ContainsKey(mode))
        {
            enterActions[mode] += action;
        }
        else 
        {
            enterActions.Add(mode, action);
        }
    }

    public void AddExitAction(GameMode mode, Action action)
    {
        if (exitActions.ContainsKey(mode))
        {
            exitActions[mode] += action;
        }
        else
        {
            exitActions.Add(mode, action);
        }
    }

    public void SetGameMode(GameMode mode)
    {
        exitActions.TryGetValue(_currentGameMode, out Action exitAction);
        exitAction?.Invoke();

        enterActions.TryGetValue(mode, out Action enterAction);
        enterAction?.Invoke();

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
