using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [System.Serializable]
    public struct ZoneRange
    {
        public int MinInclusive;
        public int MaxInclusive;

        [Range(0f, 100f)] public float Weight;
    }
}