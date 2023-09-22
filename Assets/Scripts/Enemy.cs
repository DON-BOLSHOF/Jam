using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : AliveObject
{
    [SerializeField] private Transform _player;
    private NavMeshAgent _agent;

    protected override void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        base.Start();
    }

    private void Update()
    {
        _agent.SetDestination(_player.position);
    }
}