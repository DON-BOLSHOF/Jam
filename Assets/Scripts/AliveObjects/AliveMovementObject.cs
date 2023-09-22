using System;
using UnityEngine;

namespace AliveObjects
{
    public class AliveMovementObject : AliveObject
    {
        [SerializeField] private Animator _animator;
        private int _animSpeed = Animator.StringToHash("Speed");
        private int _animStrafe = Animator.StringToHash("Strafe");
        protected Vector3 _animMov, _targetMov;

        protected virtual void LateUpdate()
        {
            Move();
        }

        private void Move()
        {
            _animMov = Vector3.Lerp(_animMov,_targetMov, Time.deltaTime * 5);
            Debug.Log(_animMov);
            _animator.SetFloat(_animSpeed, _animMov.y);
            _animator.SetFloat(_animStrafe, _animMov.x);
        }
    }
}