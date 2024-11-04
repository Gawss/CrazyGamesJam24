using UnityEngine;

namespace CrazyGames24
{
    public class Player : MonoBehaviour
    {
        [HideInInspector] public Rigidbody rb;
        [SerializeField] private LineRenderer fishingLine;

        public Fish currentFish;
        public float speed = 5f;
        public float pullingSpeed = 0f;

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void AttachFish(Fish targetFish)
        {
            if (currentFish != null) currentFish.Deattach();
            currentFish = targetFish;

        }

        private void Update()
        {
            if (currentFish == null) return;
            fishingLine.SetPosition(0, this.transform.position);
            fishingLine.SetPosition(1, currentFish.transform.position);

            transform.position = Vector3.Lerp(transform.position, transform.position + currentFish.transform.forward, Time.deltaTime * speed);
        }
    }
}