using System;
using Components;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Seed : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<HealthComponent>(out var health))
        {
            health.ModifyHealth(-_damage);
        }
        else if (collision.gameObject.TryGetComponent<GerminatingSurface>(out var surface))
        {
            surface.GerminateSurface();
        }

        Destroy(gameObject);
    }
}