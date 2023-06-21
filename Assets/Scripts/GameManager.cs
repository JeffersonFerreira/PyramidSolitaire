using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PyramidSolitaire
{
    public class GameManager : MonoBehaviour
    {
        private CardPile _drawPile;
        private CardPilePyramid _pyramid;
        private InteractionSystem _interaction;

        private void Start()
        {
            _interaction = FindObjectOfType<InteractionSystem>();

            _drawPile = CardPile.Get(CardPosition.DrawPile);
            _pyramid = CardPile.Get<CardPilePyramid>(CardPosition.Pyramid);

            InitializeGame();

            _interaction.OnPickedCards += InteractionPickedCards;
        }


        private void GameOver(bool playerWon)
        {
            Debug.Log($"GameOver: {nameof(playerWon)} = {playerWon}");
        }

        private void InitializeGame()
        {
            var deck = new Deck();
            var prefab = GameContext.Instance.CardPrefab;
            var repo = GameContext.Instance.CardRepository;
            var root = GameContext.Instance.CardsSpawnContainer;

            deck.GenerateCards(prefab, repo, root);
            deck.Shuffle();

            _pyramid.AddCard(deck.Draw(CardPilePyramid.INITIAL_CARD_COUNT));
            _drawPile.AddCard(deck.DrawRemaining());
        }

        private void InteractionPickedCards(IReadOnlyList<Card> _)
        {
            if (_pyramid.Count == 0)
            {
                GameOver(true);
            }
            else if (_drawPile.Count == 0 && !HasWinCondition())
            {
                GameOver(false);
            }
        }

        private bool HasWinCondition()
        {
            // There are valid win conditions?
            // Grab up cards from pyramid
            // Add the top card from discard pile

            // Collect the value into a hashset
            // Check if there is a match for 13


            var hashSet = _pyramid.Cards
                .Where(c => c.FaceDirection == Face.Up)
                .Select(c => c.Value)
                .ToHashSet();

            if (CardPile.Get<CardPileStack>(CardPosition.DiscardPile).TryPeek(out var card))
                hashSet.Add(card.Value);

            foreach (int value in hashSet)
            {
                if (value == 13)
                    return true;

                int remainder = 13 - value;

                if (hashSet.Contains(remainder))
                    return true;
            }

            return false;
        }
    }
}