using UnityEngine;

namespace PyramidSolitaire
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private CardRepository _cardRepository;

        [field: SerializeField]
        public Card CardPrefab { get; private set; }

        public static GameContext Instance { get; private set; }
        public CardRepository CardRepository => _cardRepository;

        private void Awake()
        {
            Instance = this;
        }
    }
}