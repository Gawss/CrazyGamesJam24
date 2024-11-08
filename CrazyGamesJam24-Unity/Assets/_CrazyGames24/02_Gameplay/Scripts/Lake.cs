using DG.Tweening;
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

            fishingSpotsParent.localPosition += Vector3.up * -6f;
            fishingSpotsParent.DOMoveY(0f, 0.75f);
        }
    }
}