using System.Collections.Generic;
using UnityEngine;

namespace PyramidSolitaire
{
    // Notice: If any of there is throwing, you probably have 2 class instances with same "Position" value
    public abstract class CardPile : MonoBehaviour
    {
        [field: SerializeField] public CardPosition Position { get; private set; }

        public static IReadOnlyDictionary<CardPosition, CardPile> All => _allPiles;
        private static readonly Dictionary<CardPosition, CardPile> _allPiles = new();

        public abstract int Count { get; }

        protected virtual void OnEnable()
        {
            _allPiles.Add(Position, this);
        }

        protected virtual void OnDisable()
        {
            _allPiles.Remove(Position);
        }

        public abstract void AddCard(params Card[] cards);
        public abstract bool TryRemove(Card targetCard);

        public static CardPile Get(CardPosition position)
        {
            if (!_allPiles.TryGetValue(position, out var pile))
            {
                Debug.LogError($"Unable to find pile at '{position}'");
                return null;
            }

            return pile;
        }

        public static T Get<T>(CardPosition position) where T : CardPile
        {
            if (Get(position) is not T pileTyped)
            {
                Debug.LogError($"Pile at position '{position}' is not of type {typeof(T).Name}");
                return null;
            }

            return pileTyped;
        }
    }
}