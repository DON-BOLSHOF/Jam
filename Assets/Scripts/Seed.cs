using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Seed : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        
        if (collision.gameObject.TryGetComponent<HealthComponent>(out var health))
        {
            health.ModifyHealth(-_damage);
        }

        Destroy(gameObject);
    }
}