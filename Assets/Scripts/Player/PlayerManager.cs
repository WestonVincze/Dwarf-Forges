using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Handles inputs and actions related to direct player control within the game
 * (switching game modes, drag/drop, etc)
 */
public class PlayerManager: MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private InputAction _craftingModeAction;
    private InputAction _pauseMenuAction;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _craftingModeAction = _playerInputActions.GameControls.CraftingMode;
        _craftingModeAction.Enable();
        _craftingModeAction.performed += toggleCraftingMode => GameManager.instance.ToggleGameMode(GameManager.GameMode.Crafting);

        _pauseMenuAction = _playerInputActions.GameControls.PauseMenu;
        _pauseMenuAction.Enable();
        _pauseMenuAction.performed += togglePauseMenu => GameManager.instance.ToggleGameMode(GameManager.GameMode.PauseMenu);
    }

    private void OnDisable()
    {
        _craftingModeAction.Disable();
        _pauseMenuAction.Disable();
    }
}
