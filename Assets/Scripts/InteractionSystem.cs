using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PyramidSolitaire
{
    public class InteractionSystem : MonoBehaviour
    {
        public event Action<IReadOnlyList<Card>> OnPickedCards;
        public event Action<Card> OnDrawCard;

        public IReadOnlyList<Card> SelectedCards => _selectedCards;

        private readonly List<Card> _selectedCards = new(2);

        private Camera _cam;

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
                ClickOnCard(card);
            }
            else
            {
                ClearSelection();
            }
        }

        public void ClickOnCard(Card card)
        {
            switch (card.Position)
            {
                case CardPosition.DrawPile:
                {
                    CardPileStack discardPile = CardPile.Get<CardPileStack>(CardPosition.DiscardPile);

                    card.Flip(Face.Up);
                    discardPile.AddCardAnimated(card);

                    OnDrawCard?.Invoke(card);
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

        public void ClearSelection()
        {
            _selectedCards.ForEach(c => c.SetSelected(false));
            _selectedCards.Clear();
        }

        private void SelectCard(Card selectedCard)
        {
            _selectedCards.Add(selectedCard);
            selectedCard.SetSelected(true);

            if (_selectedCards.Sum(c => c.Value) == 13)
            {
                var pairPile = CardPile.Get<CardPileStack>(CardPosition.PairedPile);

                // Move selected cards into "paired" pile
                foreach (var card in _selectedCards)
                {
                    card.TryDisconnectAndShowAbove();
                    pairPile.AddCardAnimated(card);
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
    }
}