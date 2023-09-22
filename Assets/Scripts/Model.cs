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
    private Vector3 _lookRot;
    [SerializeField] private float _rotationSpeed;
    private Vector2 _currentMouseDelta;
    private float _cameraCap;
    private Vector2 _rotationInput;

    [SerializeField] private Animator _animator;
    private int _animSpeed = Animator.StringToHash("Speed");
    private Transform _cameraTransform;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _playerInput.OnGroundedMoved.Subscribe(UpdateMovement).AddTo(this);
        _playerInput.OnJumped.Subscribe(value => _jumpVelocity = value * jumpForce).AddTo(this);
        _playerInput.OnMouseMoved.Subscribe(value => _rotationInput = value).AddTo(this);
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (_controller.isGrounded && _jumpVelocity > 0)
            _moveDirection.y = _jumpVelocity;
        
        _moveDirection.y -= gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
            GetDamage(5);
        
        RotateOnLookTransform();
    }

    private void LateUpdate()
    {
        _lookRot.y += _rotationInput.x;
        _lookRot.x -= _rotationInput.y;
        _lookRot.x = Mathf.Clamp(_lookRot.x, -30, 60);

        var rotation = Quaternion.Lerp(_lookTransform.rotation, Quaternion.Euler(_lookRot), Time.deltaTime * 10);
        _lookTransform.rotation = rotation;
    }

    private void UpdateMovement(Vector2 mov)
    {
        _moveDirection = _cameraTransform.localToWorldMatrix * (new Vector3(mov.x, 0, mov.y)) * moveSpeed;
        _animator.SetFloat(_animSpeed, mov.y);
    }

    private void RotateOnLookTransform()
    {
        Vector3 tempRot = _lookRot;
        tempRot.x = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(tempRot), 50);
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