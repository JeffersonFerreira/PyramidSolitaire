using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PyramidSolitaire
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            var deck = new Deck();
            var cardPrefab = GameContext.Instance.CardPrefab;
            var cardRepository = GameContext.Instance.CardRepository;

            deck.GenerateCards(cardPrefab, cardRepository);
            deck.Shuffle();

            var pyramid = FindObjectOfType<CardPyramidGenerator>();
            var drawPile = CardPile.AtPos(CardPosition.DrawPile);

            pyramid.Generate(deck.Draw(28)); // TODO: Create a constant number somewhere
            drawPile.AddCard(deck.DrawRemaining());
        }
    }

    public class Deck
    {
        private readonly List<Card> _cardList = new();

        public void GenerateCards(Card cardPrefab, CardRepository cardRepository)
        {
            foreach (CardData cardData in cardRepository)
            {
                var card = Object.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);

                card.Setup(cardData);
                card.SetVisibility(false);
                _cardList.Add(card);
            }
        }

        public void Shuffle()
        {
            Debug.LogWarning("Deck.Shuffle() was not implemented");
        }

        public Card[] Draw(int amount)
        {
            Card[] cards = _cardList.Take(amount).ToArray();
            _cardList.RemoveRange(0, cards.Length);

            return cards;
        }

        public Card[] DrawRemaining()
        {
            Card[] cards = _cardList.ToArray();
            _cardList.Clear();
            return cards;
        }
    }
}