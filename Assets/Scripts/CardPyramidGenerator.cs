using System.Collections.Generic;
using UnityEngine;

namespace PyramidSolitaire
{
    public class CardPyramidGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 _cardSize = new Vector2(1, 1);
        [SerializeField] private Vector2 _cardSpacing = Vector2.zero;

        private readonly List<PyramidPos> _generatedPositions = new(27);

        private const int TOTAL_ROWS = 7;

        public void Generate(Card[] cards)
        {
            GeneratePositions();

            for (var i = 0; i < cards.Length; i++)
            {
                PyramidPos genPosData = _generatedPositions[i];
                Card card = cards[i];

                Vector3 pos = genPosData.GlobalPosition;
                pos.z = -genPosData.Row;

                card.transform.position = pos;

                card.SetVisibility(true);
                card.SetInteractable(genPosData.Row == TOTAL_ROWS - 1);
            }
        }

        private void GeneratePositions(bool includeOffset = true)
        {
            _generatedPositions.Clear();

            var center = includeOffset ? transform.position : Vector3.zero;
            for (int row = 0; row < TOTAL_ROWS; row++)
            {
                int cardCount = row + 1;
                for (int col = 0; col < cardCount; col++)
                {
                    var pos = new Vector2(
                        (center.x + _cardSize.x * col) + (_cardSpacing.x * col) -
                        ((_cardSize.x + _cardSpacing.x) * row * 0.5f),
                        (center.y - _cardSize.y * row) + (_cardSpacing.y * row)
                    );

                    _generatedPositions.Add(new PyramidPos
                    {
                        Row = row,
                        Column = col,
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
            public int Column;
        }

    }
}