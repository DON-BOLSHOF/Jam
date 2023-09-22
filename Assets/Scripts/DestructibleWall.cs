using UniRx;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class DestructibleWall : AliveObject
{
    private DestructibleWallState _destructibleWallState = DestructibleWallState.Alive;

    protected override void Death()
    {
        _destructibleWallState = DestructibleWallState.Destroyed;
        
        base.Death();
    }
}

public enum DestructibleWallState
{
    Alive,
    Destroyed
}
