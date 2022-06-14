using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour {

        float waypointGizmoRadius = 0.5f;

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextWaypoint(i)));
            }
        }

        public int GetNextWaypoint(int i)
        {
            if (i < (transform.childCount - 1))
            {
                return i + 1;
            } else {
                return 0;
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}