using System;
using AliveObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GerminatingSurface : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    
    private ReactiveProperty<GerminatingSurfaceState> _germinatingState;

    private void Start()
    {
        Enemy enemy = null;
        
        gameObject.OnCollisionEnterAsObservable()
            .Where(_ => _germinatingState.Value == GerminatingSurfaceState.Germinated)
            .Where(collision => collision.gameObject.layer == _layerMask)
            .Where(collision => collision.gameObject.TryGetComponent(out enemy))
            .Subscribe(value => CorruptEntity(enemy)).AddTo(this);
    }

    private void CorruptEntity(Enemy enemy)
    {
        enemy.Stun();
    }

    public void GerminateSurface()
    {
        _germinatingState.Value = GerminatingSurfaceState.Germinated;
    }
}

public enum GerminatingSurfaceState
{
    Empty,
    Germinated
}