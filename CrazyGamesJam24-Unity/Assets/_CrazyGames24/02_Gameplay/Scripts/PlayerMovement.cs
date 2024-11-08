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
        [SerializeField] private GameObject paddle_left;
        [SerializeField] private GameObject paddle_right;

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
            if (!GameManager.Instance.GameRunning) return;
            if (player.isFishing) return;


            direction.x = vector.x;
            direction.z = vector.y;

            player.SetCharacter(true, direction.x < 0 ? true : false);

            if (direction.magnitude > 0.1f)
            {
                currentRotation = Quaternion.LookRotation(direction);
                currentRotation = currentRotation * Quaternion.Euler(cameraOffset);
            }

            if (direction.x < 0)
            {
                if (player.paddle != paddle_left) player.paddle.SetActive(false);
                player.paddle = paddle_left;
                player.paddle.SetActive(true);
            }
            else
            {
                if (player.paddle != paddle_right) player.paddle.SetActive(false);
                player.paddle = paddle_right;
                player.paddle.SetActive(true);
            }

        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.GameRunning) return;
            if (player.isFishing) return;

            player.rb.rotation = Quaternion.Slerp(player.rb.rotation, currentRotation, Time.deltaTime * 5f);

            Vector3 movement = transform.forward * direction.magnitude * moveSpeed * Time.deltaTime;

            if (movement.magnitude <= 0.1f) player.SetCharacter(false);
            player.rb.MovePosition(player.rb.position + movement);
        }
    }
}