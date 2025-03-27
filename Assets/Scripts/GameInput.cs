using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnReloadAction;
    public event EventHandler OnSprintStartedAction;
    public event EventHandler OnSprintCanceledAction;
    public event EventHandler OnShootingStartedAction;
    public event EventHandler OnShootingCanceledAction;

    private InputManager inputManager;

    private Vector2 inputVector;

    private void Awake()
    {
        Instance = this;

        inputManager = new InputManager();
        inputManager.Player.Enable();

        inputManager.Player.Reload.performed += Reload_performed;
        inputManager.Player.Sprint.started += Sprint_started;
        inputManager.Player.Sprint.canceled += Sprint_canceled;
        inputManager.Player.Shoot.started += Shoot_started;
        inputManager.Player.Shoot.canceled += Shoot_canceled;
    }

    private void OnDestroy()
    {
        inputManager.Player.Reload.performed -= Reload_performed;
        inputManager.Player.Sprint.started -= Sprint_started;
        inputManager.Player.Sprint.canceled -= Sprint_canceled;
        inputManager.Player.Shoot.started -= Shoot_started;
        inputManager.Player.Shoot.canceled -= Shoot_canceled;
    }

    public Vector3 GetMovementVector()
    {
        inputVector = inputManager.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();

        return new Vector3(inputVector.x, 0, inputVector.y);
    }

    public Vector2 GetMousePosition()
    {
        return inputManager.Player.MousePosition.ReadValue<Vector2>();
    }

    private void Reload_performed(InputAction.CallbackContext obj)
    {
        OnReloadAction?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_canceled(InputAction.CallbackContext obj)
    {
        OnSprintCanceledAction?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_started(InputAction.CallbackContext obj)
    {
        OnSprintStartedAction?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_canceled(InputAction.CallbackContext obj)
    {
        OnShootingCanceledAction?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
        OnShootingStartedAction?.Invoke(this, EventArgs.Empty);
    }
}
