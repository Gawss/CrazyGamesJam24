using UnityEngine;

namespace CrazyGames24
{
    public class CameraTarget : MonoBehaviour
    {
        Vector3 targetPositon = Vector3.zero;
        Vector3 initialOffset;
        Transform currentTarget;

        public void SetTarget(Transform target)
        {
            currentTarget = target;
            targetPositon.z = transform.position.z + target.position.z;
        }

        private void Start()
        {
            initialOffset = GameManager.Instance.player.transform.position;
            targetPositon = transform.position;
            SetTarget(GameManager.Instance.player.transform);
        }

        void Update()
        {
            targetPositon.z = Mathf.Max(-20f, Mathf.Min(590, currentTarget.position.z + initialOffset.z));
            transform.position = targetPositon;
        }
    }
}