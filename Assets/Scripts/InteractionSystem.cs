using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PyramidSolitaire
{
    public class InteractionSystem : MonoBehaviour
    {
        private Camera _cam;

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
                HandleCardHit(card);
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
                    // TODO: Only process if is a leaf card
                    MarkAsSelected(card);
                    break;
                }
                case CardPosition.DiscardPile:
                {
                    // Select this card
                    MarkAsSelected(card);
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

        private void MarkAsSelected(Card card)
        {
            _selectedCards.Add(card);
            card.SetSelected(true);

            if (_selectedCards.Sum(c => c.Value) == 13)
            {
                var pile = CardPile.Get(CardPosition.PairedPile);

                // Move selection to "paired" pile
                foreach (var c in _selectedCards)
                {
                    c.SetSelected(false);
                    pile.AddCard(c);
                }

                _selectedCards.Clear();
            }
            else if (_selectedCards.Count == 2)
            {
                // Clear selection
                foreach (var c in _selectedCards)
                    c.SetSelected(false);

                _selectedCards.Clear();
            }
        }
    }
}