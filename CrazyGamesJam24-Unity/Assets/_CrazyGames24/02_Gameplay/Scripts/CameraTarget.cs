using UnityEngine;

namespace CrazyGames24
{
    public class CameraTarget : MonoBehaviour
    {
        Vector3 targetPositon = Vector3.zero;

        Vector3 initialOffset;

        private void Start()
        {
            initialOffset = GameManager.Instance.player.transform.position;
            targetPositon = transform.position + initialOffset;
        }

        void Update()
        {
            targetPositon.z = Mathf.Max(-20f, Mathf.Min(590, GameManager.Instance.player.transform.position.z + initialOffset.z));
            transform.position = targetPositon;
        }
    }
}