using System.Collections.Generic;
using System.Linq;
using PyramidSolitaire.Extensions;
using UnityEngine;

namespace PyramidSolitaire
{
    public class GameManager : MonoBehaviour
    {
        private CardPile _drawPile;
        private CardPilePyramid _pyramid;
        private InteractionSystem _interaction;

        private GameUIManager _gameUI;
        private readonly HashSet<int> _cardsValueSet = new();

        private void Awake()
        {
            _gameUI = FindObjectOfType<GameUIManager>();
        }

        private void Start()
        {
            _interaction = FindObjectOfType<InteractionSystem>();

            _drawPile = CardPile.Get(CardPosition.DrawPile);
            _pyramid = CardPile.Get<CardPilePyramid>(CardPosition.Pyramid);

            InitializeGame();

            _interaction.OnPickedCards += UserInteraction_PickedCards;
        }


        private void GameOver(bool playerWon)
        {
            _gameUI.ShowGameOver(playerWon);
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

        private void UserInteraction_PickedCards(IReadOnlyList<Card> _)
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
            _cardsValueSet.Clear();
            _pyramid.Cards
                .Where(c => c.FaceDirection == Face.Up)
                .Select(c => c.Value)
                .AddTo(_cardsValueSet);

            if (CardPile.Get<CardPileStack>(CardPosition.DiscardPile).TryPeek(out var card))
                _cardsValueSet.Add(card.Value);

            foreach (int value in _cardsValueSet)
            {
                if (value == 13)
                    return true;

                int remainder = 13 - value;

                // 13 is a prime number, we will not face an issue like "value + value == 13"
                if (_cardsValueSet.Contains(remainder))
                    return true;
            }

            return false;
        }
    }
}