using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    private DestructibleWallState _destructibleWallState = DestructibleWallState.Alive;

    [SerializeField] private IntReactiveProperty _health;

    private void Start()
    {
        _health.Where(value => value <= 0).Subscribe(_=>Destroy(gameObject)).AddTo(this);
    }

    public void GetDamage(int value)
    {
        _health.Value -= value;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            GetDamage(5);
    }
}

public enum DestructibleWallState
{
    Alive,
    Destroyed
}
