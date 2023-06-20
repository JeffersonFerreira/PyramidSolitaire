using UnityEngine;

namespace PyramidSolitaire
{
    [CreateAssetMenu(fileName = "CardRepository", menuName = "CardRepository", order = 0)]
    public class CardRepository : ScriptableObject
    {
        [SerializeField] private Data[] _datas;

        [System.Serializable]
        public class Data
        {
            public Sprite FrontFace;
            public int Value;
        }
    }
}