using UnityEngine;
using UnityEngine.AI;

namespace AliveObjects
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : AliveMovementObject
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

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.isStopped = true;
                
                _targetMov = Vector3.zero;
                _animMov = Vector3.zero;
            }
            else
            {
                _agent.isStopped = false;
                
                _targetMov = _player.position;
            }
        }

        public void Stun()
        {
            
        }
    }
}