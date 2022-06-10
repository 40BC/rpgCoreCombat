using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float chaseDistance = 5f;
        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;
        Vector3 guardPosition;

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
                AttackPlayer();
            } else {
                GetComponent<Mover>().StartMoveAction(guardPosition);
            }
        }
        private void AttackPlayer()
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