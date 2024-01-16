using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool _isPaused;

    private bool _inDebugMode = false;

    public bool inDebugMode
    {
        get => _inDebugMode;
    }

    private Action _debugEnterActions;
    private Action _debugExitActions;

    // TODO: default mode should be main menu
    private GameMode _currentGameMode = GameMode.Normal;
    public GameMode currentGameMode
    {
        get { return _currentGameMode; }
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

    private Dictionary<GameMode, Action> _enterActions = new Dictionary<GameMode, Action>();
    private Dictionary<GameMode, Action> _exitActions = new Dictionary<GameMode, Action>();

    private void OnEnable()
    {
        _enterActions.Add(GameMode.PauseMenu, PauseGame);
        _exitActions.Add(GameMode.PauseMenu, ResumeGame);
    }

    private void OnDisable()
    {
        _enterActions.Clear();
        _exitActions.Clear();
    }

    private void Awake()
    {
        // not the best, but it should be fine for now
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddEnterAction(GameMode mode, Action action)
    {
        if (_enterActions.ContainsKey(mode))
        {
            _enterActions[mode] += action;
        }
        else
        {
            _enterActions.Add(mode, action);
        }
    }

    public void AddExitAction(GameMode mode, Action action)
    {
        if (_exitActions.ContainsKey(mode))
        {
            _exitActions[mode] += action;
        }
        else
        {
            _exitActions.Add(mode, action);
        }
    }

    public void SetGameMode(GameMode mode)
    {
        _exitActions.TryGetValue(_currentGameMode, out Action exitAction);
        exitAction?.Invoke();

        _enterActions.TryGetValue(mode, out Action enterAction);
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

    public void AddDebugEnterAction(Action action)
    {
        _debugEnterActions += action;
    }

    public void AddDebugExitAction(Action action)
    {
        _debugExitActions += action;
    }

    public void ToggleDebugMode()
    {
        _inDebugMode = !_inDebugMode;
        // entering debug mode
        if (_inDebugMode)
        {
            _debugEnterActions?.Invoke();
        }
        else
        {
            _debugExitActions?.Invoke();
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
