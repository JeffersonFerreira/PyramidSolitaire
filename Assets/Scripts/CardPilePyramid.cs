using System.Collections.Generic;
using PyramidSolitaire.Extensions;
using UnityEngine;

namespace PyramidSolitaire
{
    public class CardPilePyramid : CardPile
    {
        [SerializeField] private Vector2 _cardSize = new Vector2(1, 1);
        [SerializeField] private Vector2 _cardSpacing = Vector2.zero;

        public const int INITIAL_CARD_COUNT = 28;

        public IReadOnlyCollection<Card> Cards => _cardList;

        public override int Count => _cardList.Count;

        private readonly List<Card> _cardList = new(INITIAL_CARD_COUNT);
        private readonly List<PyramidPos> _generatedPositions = new(INITIAL_CARD_COUNT);

        private const int TOTAL_ROWS = 7;

        private void Start()
        {
            GeneratePositions();
        }

        public override void AddCard(params Card[] cards)
        {
            GeneratePositions();
            _cardList.AddRange(cards);

            for (var i = 0; i < cards.Length; i++)
            {
                Card card = cards[i];
                PyramidPos genPosData = _generatedPositions[i];

                // By reducing the "z" axis, the card gets closer to the camera
                //  and will be rendered on top of other cards.
                // We could achieve the same using the "sortOrder" property, but our needs is too simple for that.
                card.transform.position = genPosData.GlobalPosition.WithZ(-genPosData.Row);

                bool isBottomRow = genPosData.Row == TOTAL_ROWS - 1;
                card.SetPosition(Position);
                card.SetVisibility(true);
                card.SetInteractable(isBottomRow);

                if (isBottomRow)
                    card.Flip(Face.Up);

                // Connect this card with cards down me
                if (!isBottomRow)
                {
                    int rowWidth = genPosData.Row + 1;

                    Card cardDownLeft = cards[i + rowWidth];
                    Card cardDownRight = cards[i + rowWidth + 1];

                    cardDownLeft.ConnUp.Add(card);
                    cardDownRight.ConnUp.Add(card);

                    card.ConnDown.Add(cardDownLeft);
                    card.ConnDown.Add(cardDownRight);
                }
            }
        }

        public override bool TryRemove(Card targetCard)
        {
            return _cardList.Remove(targetCard);
        }

        private void GeneratePositions(bool includeOffset = true)
        {
            _generatedPositions.Clear();

            var center = includeOffset ? transform.position : Vector3.zero;
            for (int row = 0; row < TOTAL_ROWS; row++)
            {
                int rowLength = row + 1;
                for (int col = 0; col < rowLength; col++)
                {
                    float horizontalOffset = (_cardSize.x + _cardSpacing.x) * row * 0.5f;

                    var pos = new Vector2(
                        // Card Pos + Space Between - Horizontal half shift to left
                        (center.x + _cardSize.x * col) + (_cardSpacing.x * col) - horizontalOffset,
                        (center.y - _cardSize.y * row) + (_cardSpacing.y * row)
                    );

                    _generatedPositions.Add(new PyramidPos {
                        Row = row,
                        GlobalPosition = pos
                    });
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            GeneratePositions(true);

            foreach (PyramidPos pos in _generatedPositions)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, (float)pos.Row / TOTAL_ROWS);
                Gizmos.DrawWireCube(pos.GlobalPosition, _cardSize);
            }
        }

        private struct PyramidPos
        {
            public Vector2 GlobalPosition;
            public int Row;
        }
    }
}