﻿using UniRx;
using UnityEngine;

namespace AliveObjects
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class Hero : AliveObject
    {
        [SerializeField] private float moveSpeed = 5.0f; // Скорость движения
        [SerializeField] private float jumpForce = 8.0f; // Сила прыжка
        [SerializeField] private float gravity = 20.0f; // Гравитация

        private PlayerInput _playerInput;
        private CharacterController _controller;

        private Vector3 _moveDirection = Vector3.zero;
        private float _jumpVelocity = 0;

        [SerializeField] private Transform _lookTransform;
        private Vector3 _lookRot;
        private Vector2 _rotationInput;

        [SerializeField] private Animator _animator;
        private int _animSpeed = Animator.StringToHash("Speed");
        private int _animStrafe = Animator.StringToHash("Strafe");
        private Vector2 _animMov, _targetMov;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();

            _playerInput.OnGroundedMoved.Subscribe(UpdateMovement).AddTo(this);
            _playerInput.OnJumped.Subscribe(value => _jumpVelocity = value * jumpForce).AddTo(this);
            _playerInput.OnMouseMoved.Subscribe(value => _rotationInput = value).AddTo(this);
        }

        private void Update()
        {
            if (_controller.isGrounded && _jumpVelocity > 0)
                _moveDirection.y = _jumpVelocity;
        
            _moveDirection.y -= gravity * Time.deltaTime;

            RotateOnLookTransform();
        
            _animMov = Vector2.Lerp(_animMov,_targetMov, Time.deltaTime * 5);
            _animator.SetFloat(_animSpeed, _animMov.y);
            _animator.SetFloat(_animStrafe, _animMov.x);
        
            _controller.Move(transform.localToWorldMatrix * _moveDirection * Time.deltaTime);
        }

        private void LateUpdate()
        {
            _lookRot.y += _rotationInput.x;
            _lookRot.x -= _rotationInput.y;
            _lookRot.x = Mathf.Clamp(_lookRot.x, -30, 60);

            var rotation = Quaternion.Lerp(_lookTransform.rotation, Quaternion.Euler(_lookRot), .25f);
            _lookTransform.rotation = rotation;
        }

        private void UpdateMovement(Vector2 mov)
        {
            _targetMov = (new Vector3(mov.x, mov.y)) * moveSpeed;
            _moveDirection = (new Vector3(mov.x, 0, mov.y)) * moveSpeed;
        }

        private void RotateOnLookTransform()
        {
            Vector3 tempRot = _lookRot;
            tempRot.x = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(tempRot), 50);
        }
    }
}