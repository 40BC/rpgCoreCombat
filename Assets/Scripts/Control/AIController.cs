using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float chaseDistance = 5f;
        Fighter fighter;
        GameObject player;
        Health health;

        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update() 
        {
            if (health.IsDead()) return;

            if (IsPlayerInRange()) {
                AttackPlayer();
            } else {
                fighter.Cancel();
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
    }
}