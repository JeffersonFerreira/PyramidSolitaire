using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace PyramidSolitaire
{
    public enum CardPosition
    {
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

        public CardPosition Position { get; private set; }
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
            var euler = Vector3.zero;
            euler.y = face == Face.Up ? 0 : 180;
            gameObject.transform.localEulerAngles = euler;
        }

        public void LeaveCurrentPile()
        {
            // This card can only be selected if currently positioned on top of a pile.
            if (Position is not (CardPosition.DrawPile or CardPosition.DiscardPile))
                return;

            if (CardPile.Get(Position).TryDraw(out var other))
            {
                // Just a warning in case Jeff built something wrong 👀
                if (other != this)
                    throw new Exception("You are doing something wrong bro, how this happened???");
            }
        }
    }
}