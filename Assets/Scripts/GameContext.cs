using UnityEngine;

namespace PyramidSolitaire
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private CardRepository _cardRepository;
        [SerializeField] private Transform _cardsSpawnContainer;

        public Card CardPrefab => _cardPrefab;
        public CardRepository CardRepository => _cardRepository;
        public Transform CardsSpawnContainer => _cardsSpawnContainer;

        public static GameContext Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}