using UnityEngine;

namespace CrazyGames24
{
    public class Lake : MonoBehaviour
    {
        [SerializeField] private Transform fishingSpotsParent;

        Fish[] fishingSpots;
        private void OnEnable()
        {

            fishingSpots = fishingSpotsParent.GetComponentsInChildren<Fish>(true);

            foreach (Fish fpot in fishingSpots)
            {

            }
        }
    }
}