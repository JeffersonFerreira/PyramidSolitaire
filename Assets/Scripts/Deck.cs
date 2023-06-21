using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace PyramidSolitaire
{
    public class Deck
    {
        private readonly List<Card> _cardList = new();
        private Random _random;

        public void GenerateCards(Card cardPrefab, CardRepository cardRepository, Transform root, int? seed = null)
        {
            _random = !seed.HasValue
                ? new Random()
                : new Random(seed.Value);

            foreach (CardData cardData in cardRepository)
            {
                Card card = Object.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, root);
                card.gameObject.name = $"Card | {cardData.FrontFace.name}_{cardData.Value}";

                card.Setup(cardData);
                card.Flip(Face.Down);
                card.SetVisibility(false);
                _cardList.Add(card);
            }
        }

        public void Shuffle()
        {
            // Source: https://stackoverflow.com/questions/273313/randomize-a-listt

            int n = _cardList.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                Card value = _cardList[k];

                _cardList[k] = _cardList[n];
                _cardList[n] = value;
            }
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