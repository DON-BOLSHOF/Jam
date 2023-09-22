using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

namespace AliveObjects
{
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
            
            if(_agent.remainingDistance <= _agent.stoppingDistance) Debug.Log("agent");
        }

        private void Move()
        {
            
        }

        public void Stun()
        {
            
        }
    }
}