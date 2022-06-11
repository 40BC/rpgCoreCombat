using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start() {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            player = GameObject.FindGameObjectWithTag("Player");

            // Initial position of the AI to remember as guard position
            guardPosition = transform.position;
        }

        private void Update() 
        {
            if (health.IsDead()) return;

            if (IsPlayerInRange()) {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            } 
            else if (!IsPlayerInRange() || !fighter.CanAttack(player.gameObject))
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            if (timeSinceLastSawPlayer > suspicionTime) {
                GuardBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (fighter.CanAttack(player.gameObject)) 
            {
                fighter.Attack(player.gameObject);
            }        
        }

        private bool IsPlayerInRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}