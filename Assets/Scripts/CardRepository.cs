using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PyramidSolitaire
{
    [CreateAssetMenu(fileName = "CardRepository", menuName = "CardRepository", order = 0)]
    public class CardRepository : ScriptableObject, IEnumerable<CardData>
    {
        [SerializeField] private CardData[] _datas;

        public int CardCount => _datas.Length;

        IEnumerator<CardData> IEnumerable<CardData>.GetEnumerator()
        {
            return ((IEnumerable<CardData>)_datas).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => _datas.GetEnumerator();
    }

    [System.Serializable]
    public class CardData
    {
        public Sprite FrontFace;
        public int Value;
    }
}