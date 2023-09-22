using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class Model : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f; // Скорость движения
    [SerializeField] private float jumpForce = 8.0f; // Сила прыжка
    [SerializeField] private float gravity = 20.0f; // Гравитация

    private readonly ReactiveProperty<int> _health = new(100);
    public IObservable<int> HealthObservable => _health.AsObservable();

    private PlayerInput _playerInput;
    private CharacterController _controller;

    private Vector3 _moveDirection = Vector3.zero;
    private float _jumpVelocity = 0;

    [SerializeField] private Transform _lookTransform;
    [SerializeField] private float _rotationSpeed;
    private Vector2 _currentMouseDelta;
    private float _cameraCap;
    private Vector2 _rotationInput;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _playerInput.OnGroundedMoved.Subscribe(vector2 =>
                _moveDirection = transform.TransformDirection(new Vector3(vector2.x, 0, vector2.y)) * moveSpeed)
            .AddTo(this);
        _playerInput.OnJumped.Subscribe(value => _jumpVelocity = value * jumpForce).AddTo(this);
        _playerInput.OnMouseMoved.Subscribe(value => _rotationInput = value).AddTo(this);
    }

    private void Update()
    {
        if (_controller.isGrounded && _jumpVelocity > 0)
        {
            _moveDirection.y = _jumpVelocity;
        }
        
        _moveDirection.y -= gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
            GetDamage(5);
    }

    private void LateUpdate()
    {
        Vector3 rot = _lookTransform.rotation.eulerAngles;

        rot.y += _rotationInput.x;
        rot.x += _rotationInput.y;

        var rotation = Quaternion.Euler(rot);
        _lookTransform.rotation = rotation;
    }

    private void GetDamage(int value)
    {
        _health.Value -= value;
    }
    
    private void FixedUpdate()
    {
        _controller.Move(_moveDirection * Time.deltaTime);
    }
}