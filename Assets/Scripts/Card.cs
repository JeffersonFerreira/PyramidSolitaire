using System;
using UnityEngine;

namespace PyramidSolitaire
{
    public enum CardPosition
    {
        Pyramid,
        DrawPile,
        DiscardPile,
        PairedPile
    }

    public class Card : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _frontFaceSprite;

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
    }
}