using System;
using UnityEngine;

namespace CrazyGames24
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 3f;
        Vector3 targetPosition;
        Vector3 direction;
        Quaternion currentRotation;
        Player player;

        [SerializeField] private Vector3 cameraOffset;

        private void Start()
        {
            player = GetComponent<Player>();
            direction = Vector3.zero;
            currentRotation = Quaternion.identity;

            GameManager.Instance.inputManager.OnMovePerformed += Move;
        }

        private void OnDestroy()
        {
            GameManager.Instance.inputManager.OnMovePerformed -= Move;
        }

        private void Move(Vector2 vector)
        {
            if (player.isFishing) return;

            player.SetCharacter(true);

            direction.x = vector.x;
            direction.z = vector.y;

            if (direction.magnitude > 0.1f)
            {
                currentRotation = Quaternion.LookRotation(direction);
                currentRotation = currentRotation * Quaternion.Euler(cameraOffset);
            }

        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.GameRunning) return;
            if (player.isFishing) return;

            player.rb.rotation = Quaternion.Slerp(player.rb.rotation, currentRotation, Time.deltaTime * 25f);

            Vector3 movement = transform.forward * direction.magnitude * moveSpeed * Time.deltaTime;

            if (movement.magnitude <= 0.1f) player.SetCharacter(false);
            player.rb.MovePosition(player.rb.position + movement);
        }
    }
}