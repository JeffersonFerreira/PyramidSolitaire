using System;
using System.Collections.Generic;
using System.Linq;
using PyramidSolitaire.Extensions;
using UnityEngine;

namespace PyramidSolitaire
{
    public class GameManager : MonoBehaviour
    {
        public event Action<bool> OnGameOver;

        private CardPile _drawPile;
        private CardPilePyramid _pyramid;
        private CardPileStack _discardPile;
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
            _discardPile = CardPile.Get<CardPileStack>(CardPosition.DiscardPile);

            InitializeGame();

            _interaction.OnPickedCards += UserInteraction_PickedCards;
            _interaction.OnDrawCard += UserInteraction_DrawCard;
        }


        private void GameOver(bool playerWon)
        {
            OnGameOver?.Invoke(playerWon);
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

        private void UserInteraction_DrawCard(Card _)
        {
            if (_drawPile.Count == 0 && !HasWinCondition())
                GameOver(false);
        }

        private bool HasWinCondition()
        {
            return GameAutoMatcher.GetMatchingCards(_pyramid, _discardPile, out _) > 0;
        }
    }
}