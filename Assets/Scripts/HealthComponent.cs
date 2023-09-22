using System;
using UniRx;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private IntReactiveProperty _health;
    public IObservable<int> HealthObservable => _health.AsObservable();

    public void ModifyHealth(int value)
    {
        _health.Value += value;
    }
}