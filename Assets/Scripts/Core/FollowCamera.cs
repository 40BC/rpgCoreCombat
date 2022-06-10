using UnityEngine;

namespace RPG.Core
{
        public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float sensitivity;
        void LateUpdate()
        {
            transform.position = target.position;
            Camera.main.fieldOfView += (Input.mouseScrollDelta.y * -1) * sensitivity;
        }
    }
}