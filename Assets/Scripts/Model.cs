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

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _playerInput.OnMoved.Subscribe(vector2 =>
                _moveDirection = transform.TransformDirection(new Vector3(vector2.x, 0, vector2.y)) * moveSpeed)
            .AddTo(this);
        _playerInput.OnJumped.Subscribe(value => _jumpVelocity = value * jumpForce).AddTo(this);
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
        
        if (new Vector2(_moveDirection.x, _moveDirection.z).sqrMagnitude == 0) return;

        float currentVelocity = 0;
        var targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, 0.05f);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}