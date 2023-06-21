using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PyramidSolitaire
{
    public class InteractionSystem : MonoBehaviour
    {
        private Camera _cam;

        public event Action<IReadOnlyList<Card>> OnPickedCards;

        private readonly List<Card> _selectedCards = new(2);

        private void Start()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            var point = _cam.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.GetRayIntersection(new Ray(point, Vector3.forward));

            if (hit.transform != null && hit.transform.TryGetComponent(out Card card))
            {
                HandleCardHit(card);
            }
            else
            {
                ClearSelection();
            }
        }

        private void HandleCardHit(Card card)
        {
            switch (card.Position)
            {
                case CardPosition.DrawPile:
                {
                    CardPile discardPile = CardPile.Get(CardPosition.DiscardPile);

                    card.Flip(Face.Up);
                    discardPile.AddCard(card);

                    break;
                }
                case CardPosition.Pyramid:
                {
                    SelectCard(card);
                    break;
                }
                case CardPosition.DiscardPile:
                {
                    SelectCard(card);
                    break;
                }
                case CardPosition.PairedPile:
                {
                    Debug.LogWarning("Paired cards should not be selectable", card);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SelectCard(Card selectedCard)
        {
            _selectedCards.Add(selectedCard);
            selectedCard.SetSelected(true);

            if (_selectedCards.Sum(c => c.Value) == 13)
            {
                var pairPile = CardPile.Get(CardPosition.PairedPile);

                // Move selected cards into "paired" pile
                foreach (var card in _selectedCards)
                {
                    card.TryDisconnectAndShowAbove();
                    pairPile.AddCard(card);
                }

                // I know this may cause unexpected side effect for async operations, but this is not the case here.
                // Also, I don't want to instantiate a new collection just for that.
                OnPickedCards?.Invoke(_selectedCards);
                ClearSelection();
            }
            else if (_selectedCards.Count == 2)
            {
                OnPickedCards?.Invoke(_selectedCards);
                ClearSelection();
            }
        }

        private void ClearSelection()
        {
            _selectedCards.ForEach(c => c.SetSelected(false));
            _selectedCards.Clear();
        }
    }
}