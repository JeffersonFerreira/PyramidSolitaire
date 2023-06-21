using System.Collections.Generic;
using UnityEngine;

namespace PyramidSolitaire
{
    public class CardPileStack : CardPile
    {
        public override int Count => _stack.Count;
        private readonly Stack<Card> _stack = new();

        private void Awake()
        {
            // This sprite renderer is just a visualizer, so we know where the pile is
            if (TryGetComponent(out SpriteRenderer spriteRenderer))
                spriteRenderer.enabled = false;
        }

        public override void AddCard(params Card[] cards)
        {
            // Hide Top card
            if (_stack.TryPeek(out var topCard))
                topCard.SetVisibility(false);

            // Hide all cards and store into
            foreach (var card in cards)
            {
                card.transform.position = transform.position;
                card.LeaveCurrentPile();
                card.SetPosition(Position);
                card.SetVisibility(false);

                _stack.Push(card);
            }

            // Show top card
            _stack.Peek().SetVisibility(true);
        }

        public override bool TryRemove(Card targetCard)
        {
            if (!_stack.TryPeek(out var card) || targetCard != card)
                return false;

            _stack.Pop();

            if (_stack.TryPeek(out var topCard))
                topCard.SetVisibility(true);

            return true;
        }
    }
}