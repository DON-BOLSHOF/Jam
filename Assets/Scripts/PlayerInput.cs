using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;

    public ReactiveCommand<float> OnJumped = new();
    public ReactiveCommand<Vector2> OnGroundedMoved = new();
    public ReactiveCommand<Vector2> OnMouseMoved = new();

    private void Awake()
    {
        _playerInputSystem = new();
    }

    private void Start()
    {
        _playerInputSystem.Keyboard.Movement.performed += OnMove;
        _playerInputSystem.Keyboard.Movement.canceled += OnMove;  
        
        _playerInputSystem.Keyboard.Jump.performed += OnJump;
        _playerInputSystem.Keyboard.Jump.canceled += OnJump;

        _playerInputSystem.Mouse.MouseMove.performed += OnMouseMove;
        _playerInputSystem.Mouse.MouseMove.canceled += OnMouseMove;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();

        OnGroundedMoved?.Execute(value);
    }
    
    private void OnJump(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<float>();

        OnJumped?.Execute(value);
    }

    private void OnMouseMove(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        
        OnMouseMoved?.Execute(value);
    }

    private void OnEnable()
    {
        _playerInputSystem.Enable();
    }

    private void OnDisable()
    {
        _playerInputSystem.Disable();
    }

    private void OnDestroy()
    {
        _playerInputSystem.Keyboard.Movement.performed -= OnMove;
        _playerInputSystem.Keyboard.Movement.canceled -= OnMove;
        
        _playerInputSystem.Keyboard.Jump.performed -= OnJump;
        _playerInputSystem.Keyboard.Jump.canceled -= OnJump;
        
        _playerInputSystem.Mouse.MouseMove.performed -= OnMouseMove;
        _playerInputSystem.Mouse.MouseMove.canceled -= OnMouseMove;
    }
}