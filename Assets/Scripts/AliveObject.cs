using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class AliveObject : MonoBehaviour
{
    protected HealthComponent _healthComponent;

    protected virtual void Start()
    {
        _healthComponent = GetComponent<HealthComponent>();

        _healthComponent.HealthObservable.Where(value => value <= 0).Subscribe(_ => Death()).AddTo(this);
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}