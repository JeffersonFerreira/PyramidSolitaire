using UnityEngine;

namespace PyramidSolitaire
{
    public class InteractionSystem : MonoBehaviour
    {
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
                HandleCardHit(card);
        }

        private void HandleCardHit(Card card)
        {
            Debug.Log($"Card hit at '{card.Position}' with value '{card.Value}'");

            MoveCardTo(card, CardPosition.DiscardPile);
        }

        private void MoveCardTo(Card card, CardPosition position)
        {
            if (position == CardPosition.Pyramid)
            {
                Debug.LogError("Moving card to pyramid is not allowed");
                return;
            }

            var pile = CardPile.AtPos(position);

            card.transform.position = pile.transform.position;
            card.SetPosition(position);
            pile.AddCard(card);
        }
    }
}