using System.Collections.Generic;
using UnityEngine;

namespace PyramidSolitaire
{
    // Notice: If any of there is throwing, you probably have 2 class instances with same "Position" value
    public class CardPile : MonoBehaviour
    {
        [SerializeField] private CardPosition _position;

        public CardPosition Position => _position;

        public static IReadOnlyDictionary<CardPosition, CardPile> All => _internalPiles;
        private static readonly Dictionary<CardPosition, CardPile> _internalPiles = new();
        private Stack<Card> _cards = new();

        private void Awake()
        {
            // This sprite renderer is just a visualizer, so we know where the pile is
            if (TryGetComponent(out SpriteRenderer spriteRenderer))
                spriteRenderer.enabled = false;
        }

        private void OnEnable()
        {
            _internalPiles.Add(_position, this);
        }

        private void OnDisable()
        {
            _internalPiles.Remove(_position);
        }

        public void AddCard(Card card)
        {
            // TODO: Hide cards underneath
            // if (_cards.TryPeek(out var topCard))
            //     topCard.Hide();

            _cards.Push(card);
        }

        public static CardPile Get(CardPosition position)
        {
            if (!_internalPiles.TryGetValue(position, out var pile))
            {
                Debug.LogError($"Unable to find pile at '{position}'");
                return null;
            }

            return pile;
        }
    }
}