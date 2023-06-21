using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyramidSolitaire
{
    public class CardPileStack : CardPile
    {
        [SerializeField] private float _moveCardSpeed = 30;

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

        public void AddCardAnimated(Card card)
        {
            // Cache card before me to hide later
            bool hasCardBefore = _stack.TryPeek(out var cardBefore);

            // Already add the card to avoid an "out-of-sync" from other systems
            _stack.Push(card);
            card.LeaveCurrentPile();
            card.SetPosition(Position);

            if (hasCardBefore)
                card.SortingOrder = cardBefore.SortingOrder + 1;

            StartCoroutine(Animate());
            IEnumerator Animate()
            {
                // Animate
                var finalPos = transform.position;
                while (true)
                {
                    var pos = card.transform.position;
                    var newPos = Vector3.MoveTowards(pos, finalPos, Time.deltaTime * _moveCardSpeed);

                    card.transform.position = newPos;

                    if ((newPos - finalPos).sqrMagnitude < 0.1f * 0.1f)
                        break;

                    yield return null;
                }

                // Snap into final pos to avoid misalignment
                card.transform.position = finalPos;
                if (hasCardBefore)
                    cardBefore.SetVisibility(false);
            }
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

        public bool TryPeek(out Card card)
        {
            return _stack.TryPeek(out card);
        }
    }
}