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

    [SerializeField] private Transform _viewPoint;
    [SerializeField] private float _rotationSpeed;
    private Vector2 _currentMouseDelta;
    private float _cameraCap;
    private Vector2 _lineOfSight;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _playerInput.OnGroundedMoved.Subscribe(vector2 =>
                _moveDirection = transform.TransformDirection(new Vector3(vector2.x, 0, vector2.y)) * moveSpeed)
            .AddTo(this);
        _playerInput.OnJumped.Subscribe(value => _jumpVelocity = value * jumpForce).AddTo(this);
        _playerInput.OnMouseMoved.Subscribe(value => _lineOfSight = value).AddTo(this);
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

    private void GetDamage(int value)
    {
        _health.Value -= value;
    }
    
    private void FixedUpdate()
    {
        _controller.Move(_moveDirection * Time.deltaTime);
        
        var mouseViewportPosition = Camera.main.ViewportToWorldPoint(new Vector3(_lineOfSight.x, _lineOfSight.y, Camera.main.transform.position.z));
        
        var positionToLookAt = new Vector3(mouseViewportPosition.x, mouseViewportPosition.y, mouseViewportPosition.z);
        
        var targetRotation = Quaternion.LookRotation(positionToLookAt - _viewPoint.position);

        var rotation = Quaternion.Slerp(_viewPoint.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _viewPoint.rotation = rotation;
    }
}