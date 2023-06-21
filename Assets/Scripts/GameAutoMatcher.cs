using System.Collections.Generic;
using System.Linq;
using PyramidSolitaire.Extensions;

namespace PyramidSolitaire
{
    public static class GameAutoMatcher
    {
        private static readonly Dictionary<int, Card> _map = new();
        private static readonly Card[] _output = new Card[2];

        /// <summary>
        /// Scan the Pyramid and Discard for cards that sums 13
        /// </summary>
        /// <param name="pyramid"></param>
        /// <param name="discardPile"></param>
        /// <param name="output"> A shared array holding the found cards </param>
        /// <returns>Number of cards containing in <paramref name="output"/> </returns>
        public static int GetMatchingCards(CardPilePyramid pyramid, CardPileStack discardPile, out IReadOnlyList<Card> output)
        {
            // Clear shared properties
            _map.Clear();
            _output[0] = null;
            _output[1] = null;

            // Assigning here to avoid retyping 3 times
            output = _output;

            // Populate our Dictionary with selectable cards facing up
            pyramid.Cards
                .Where(c => c.FaceDirection == Face.Up)
                .ForEach(c => _map.TryAdd(c.Value, c));

            if (discardPile.TryPeek(out var discardCard))
                _map.TryAdd(discardCard.Value, discardCard);

            // Check if there is a possibility to find cards with 13 or a sum of 13
            foreach ((int value, var card) in _map)
            {
                if (value == 13)
                {
                    _output[0] = card;
                    return 1;
                }

                int remainder = 13 - value;
                if (_map.TryGetValue(remainder, out var other))
                {
                    _output[0] = card;
                    _output[1] = other;
                    return 2;
                }
            }

            return 0;
        }
    }
}