using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float attackSpeed = 4.2f;
        [SerializeField] float dwellTime = 3f;
        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;
        NavMeshAgent navMeshAgent;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int waypointIndex = 0;
        float defaultSpeed;

        private void Start() {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            player = GameObject.FindGameObjectWithTag("Player");
            navMeshAgent = GetComponent<NavMeshAgent>();

            // Initial position of the AI to remember as guard position
            guardPosition = transform.position;
            defaultSpeed = navMeshAgent.speed;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsPlayerInRange() && fighter.CanAttack(player.gameObject))
            {
                AttackBehaviour();
            }
            else if (!IsPlayerInRange() || !fighter.CanAttack(player.gameObject))
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            navMeshAgent.speed = defaultSpeed;
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint()) {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > dwellTime) {
                mover.StartMoveAction(nextPosition);
            }
        }

        private bool AtWaypoint()
        {
            if (Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance) {
                return true;
            }
            return false;
        }

        private void CycleWaypoint()
        {
           waypointIndex = patrolPath.GetNextWaypoint(waypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(waypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            if (timeSinceLastSawPlayer > suspicionTime) {
                PatrolBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            navMeshAgent.speed = attackSpeed;
            fighter.Attack(player.gameObject);
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