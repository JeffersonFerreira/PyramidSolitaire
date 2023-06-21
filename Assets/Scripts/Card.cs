using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace PyramidSolitaire
{
    public enum CardPosition
    {
        Unset = -1,
        Pyramid,
        DrawPile,
        DiscardPile,
        PairedPile
    }

    public enum Face
    {
        Up,
        Down
    }

    public class Card : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _frontFaceSprite;
        [SerializeField] private SpriteRenderer _selecedRenderer;

        // TODO: Fix that shit
        public List<Card> ConnUp = new();
        public List<Card> ConnDown = new();

        public Face FaceDirection { get; private set; } = Face.Down;

        public CardPosition Position { get; private set; } = CardPosition.Unset;
        public int Value { get; private set; }

        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void Setup(CardData cardData)
        {
            _frontFaceSprite.sprite = cardData.FrontFace;
            Value = cardData.Value;
        }

        public void SetPosition(CardPosition position)
        {
            Position = position;
        }

        public void SetInteractable(bool state)
        {
            _collider.enabled = state;
        }

        public void SetVisibility(bool state)
        {
            gameObject.SetActive(state);
        }

        public void SetSelected(bool state)
        {
            _selecedRenderer.enabled = state;
        }

        public void Flip(Face face)
        {
            FaceDirection = face;

            var euler = Vector3.zero;
            euler.y = face == Face.Up ? 0 : 180;
            gameObject.transform.localEulerAngles = euler;
        }

        public void LeaveCurrentPile()
        {
            if (Position is CardPosition.Unset or CardPosition.PairedPile)
                return;

            if (!CardPile.Get(Position).TryRemove(this))
            {
                // Just a warning in case Jeff built something wrong 👀
                throw new Exception("You are doing something wrong bro, how this happened???");
            }
        }

        public void TryDisconnectAndShowAbove()
        {
            // Try show cards blocked by this.
            foreach (var upperCard in ConnUp)
            {
                upperCard.ConnDown.Remove(this);
                if (upperCard.ConnDown.Count == 0)
                {
                    upperCard.Flip(Face.Up);
                    upperCard.SetInteractable(true);
                }
            }
        }
    }
}