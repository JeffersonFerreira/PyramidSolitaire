using System;
using System.Collections;
using System.Collections.Generic;
using PyramidSolitaire.Extensions;
using UnityEngine;

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

        [Space]
        [SerializeField] private float _flipSpeed = 30*30;

        // Jeff: This is not great, but I'm not being able to think in a good abstraction for now
        public List<Card> ConnUp = new();
        public List<Card> ConnDown = new();

        public int Value { get; private set; }
        public Face FaceDirection { get; private set; } = Face.Down;
        public CardPosition Position { get; private set; } = CardPosition.Unset;

        public int SortingOrder
        {
            get => _spriteRenderers[0].sortingOrder;
            set => _spriteRenderers.ForEach(s => s.sortingOrder = value);
        }

        private Collider2D _collider;
        private SpriteRenderer[] _spriteRenderers;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
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
            transform.localEulerAngles = euler;
        }

        public void FlipAnimated(Face face)
        {
            FaceDirection = face;
            float targetY = face == Face.Up ? 0 : 180;

            StartCoroutine(Animate());

            IEnumerator Animate()
            {
                while (true)
                {
                    float current = transform.localEulerAngles.y;
                    float angle = Mathf.MoveTowardsAngle(current, targetY, Time.deltaTime * _flipSpeed);

                    transform.localEulerAngles = Vector3.up * angle;
                    yield return null;

                    if (Mathf.Abs(targetY - angle) < 0.1f)
                        break;
                }

                // Snap in place to avoid misalignment
                transform.localEulerAngles = Vector3.up * targetY;
            }
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
                    upperCard.FlipAnimated(Face.Up);
                    upperCard.SetInteractable(true);
                }
            }
        }
    }
}