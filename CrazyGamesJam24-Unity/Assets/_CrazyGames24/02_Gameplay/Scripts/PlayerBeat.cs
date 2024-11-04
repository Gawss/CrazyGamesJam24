using UnityEngine;

namespace CrazyGames24
{
    public class PlayerBeat : MonoBehaviour
    {
        [Range(0, 1)] public float synchValue;

        Player player;

        private void OnEnable()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            if (player.currentFish == null) return;

            // player.speed = Mathf.Min(0, Mathf.Lerp(-player.currentFish.speed, player.currentFish.speed, synchValue));
            player.speed = -player.currentFish.speed * Mathf.Cos(2 * Mathf.PI * synchValue);
            player.pullingSpeed = Mathf.Lerp(-5, 5, synchValue);
        }
    }
}