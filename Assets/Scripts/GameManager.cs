using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PyramidSolitaire
{
    public class GameManager : MonoBehaviour
    {
        private const int PYRAMID_INITIAL_CARDS = 28;

        private void Start()
        {
            var deck = new Deck();
            var cardPrefab = GameContext.Instance.CardPrefab;
            var cardRepository = GameContext.Instance.CardRepository;

            deck.GenerateCards(cardPrefab, cardRepository);
            deck.Shuffle();

            var pyramid = FindObjectOfType<CardPyramidGenerator>();
            var drawPile = CardPile.Get(CardPosition.DrawPile);

            pyramid.Generate(deck.Draw(PYRAMID_INITIAL_CARDS));
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
                Card card = Object.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
                card.gameObject.name = $"Card | {cardData.FrontFace.name}_{cardData.Value}";

                card.Setup(cardData);
                card.Flip(Face.Down);
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