using System.Collections.Generic;
using UnityEngine;

namespace PyramidSolitaire
{
    // Notice: If any of there is throwing, you probably have 2 class instances with same "Position" value
    public class CardPile : MonoBehaviour
    {
        [SerializeField] private CardPosition _position;

        public CardPosition Position => _position;

        public static IReadOnlyDictionary<CardPosition, CardPile> All => _allPiles;
        private static readonly Dictionary<CardPosition, CardPile> _allPiles = new();

        private readonly Stack<Card> _cards = new();

        private void Awake()
        {
            // This sprite renderer is just a visualizer, so we know where the pile is
            if (TryGetComponent(out SpriteRenderer spriteRenderer))
                spriteRenderer.enabled = false;
        }

        private void OnEnable()
        {
            _allPiles.Add(_position, this);
        }

        private void OnDisable()
        {
            _allPiles.Remove(_position);
        }

        public void AddCard(params Card[] cards)
        {
            // Hide Top card
            if (_cards.TryPeek(out var topCard))
                topCard.SetVisibility(false);

            // Hide all cards and store into
            foreach (var card in cards)
            {
                card.transform.position = transform.position;
                card.SetVisibility(false);

                _cards.Push(card);
            }

            // Show top card
            _cards.Peek().SetVisibility(true);
        }

        public static CardPile AtPos(CardPosition position)
        {
            if (!_allPiles.TryGetValue(position, out var pile))
            {
                Debug.LogError($"Unable to find pile at '{position}'");
                return null;
            }

            return pile;
        }
    }
}