using UnityEngine;

namespace CrazyGames24
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public Player player;
        public BTBInputManager inputManager;
        public BeatDetector beatDetector;
        public BeatInterface beatInterface;
        public AudioEventManager audioEventManager;
    }
}