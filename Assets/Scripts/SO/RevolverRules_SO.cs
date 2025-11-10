using UnityEngine;
using System.Collections.Generic;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "RevolverRules_SO", menuName = "Revolver Card Game/Revolver Rules/RevolverRules_SO")]
    public class RevolverRules_SO : ScriptableObject
    {
        private static RevolverRules_SO _instance;

        [Header("MAX REWARD")]
        [SerializeField] private int maxRewardsPerZone = 8;
        [Header("PRICE")]
        [SerializeField] private int revolverGoldPrice = 100;
        [SerializeField] private int reviveGoldPrice = 25;

        [Space]
        [Header("ZONE RANGES")]
        [SerializeField] private ZoneRange[] zoneRanges;
        
        [Space]
        [Header("SAFE ZONES")]
        [SerializeField] private RevolverZone_SO[] safeZones;

        [Space]
        [Header("GOLD ZONES")]
        [SerializeField] private RevolverZone_SO[] goldZones;

        [Space]
        [Header("NORMAL ZONES")]
        [SerializeField] private RevolverZone_SO[] normalZones;


        private Dictionary<int, RevolverZone_SO> _safeZoneDict;
        private Dictionary<int, RevolverZone_SO> _goldZoneDict;

        private List<int> _safeZoneIndexes;
        private List<int> _goldZoneIndexes;

        internal int MaxRewardsPerSection => maxRewardsPerZone;
        internal int RevolverGoldPrice => revolverGoldPrice;
        internal int ReviveGoldPrice => reviveGoldPrice;

        internal int PickGameOverIndex()
        {
            float randomZone = Random.Range(0f, 100f);
            float totalWeight = 0f;
            int gameOverIndex = 0;

            Debug.Log($"Random Zone Value: {randomZone}");

            for (int i = 0; i < zoneRanges.Length; i++)
            {
                totalWeight += zoneRanges[i].Weight;
                if (randomZone < totalWeight)
                {
                    gameOverIndex = Random.Range(zoneRanges[i].MinInclusive, zoneRanges[i].MaxInclusive + 1);
                    return gameOverIndex;
                }
            }
            return 1;
        }

        internal int PickNextIndexWithin3(int currentIndex)
        {
            int start = currentIndex + 1;
            int end = currentIndex + 3;

            var _tmpCandidates = new List<int>(1);

            for (int idx = start; idx <= end; idx++)
            {
                if (_safeZoneIndexes.Contains(idx) || _goldZoneIndexes.Contains(idx)) continue;
                _tmpCandidates.Add(idx);
            }

            if (_tmpCandidates.Count == 0) return -1;

            int pick = _tmpCandidates[Random.Range(0, _tmpCandidates.Count)];
            return pick;
        }

        internal RevolverZone_SO SelectRevolverZone(int currentRevolverZoneIndex)
        {
            if(_goldZoneDict.TryGetValue(currentRevolverZoneIndex, out var zone)) return zone;

            else if (_safeZoneDict.TryGetValue(currentRevolverZoneIndex, out zone)) return zone;

            return SelectNormalZone(currentRevolverZoneIndex);
        }

        internal int GetNextSafeZoneIndex(int currentZoneIndex)
        {
            foreach (int safeZoneIndex in _safeZoneIndexes)
            {
                if (safeZoneIndex > currentZoneIndex)
                    return safeZoneIndex;
            }
            return 0;
        }

        internal int GetNextGoldZoneIndex(int currentZoneIndex)
        {
            foreach (int goldZoneIndex in _goldZoneIndexes)
            {
                if (goldZoneIndex > currentZoneIndex)
                    return goldZoneIndex;
            }
            return 0;
        }

        internal int GetMaxRewardPerZone(int currentIndex)
        {
            return (_safeZoneDict.ContainsKey(currentIndex) 
                || _goldZoneDict.ContainsKey(currentIndex)) ? maxRewardsPerZone : maxRewardsPerZone - 1;
        }

        internal void InitializeZoneDictionaries()
        {
            _safeZoneDict = new Dictionary<int, RevolverZone_SO>();
            _goldZoneDict = new Dictionary<int, RevolverZone_SO>();
            _goldZoneIndexes = new List<int>();
            _safeZoneIndexes = new List<int>();

            foreach (var safeZone in safeZones)
            {
                _safeZoneDict[safeZone.ZoneIndex] = safeZone;
                _safeZoneIndexes.Add(safeZone.ZoneIndex);
            }
            foreach (var goldZone in goldZones)
            {
                _goldZoneDict[goldZone.ZoneIndex] = goldZone;
                _goldZoneIndexes.Add(goldZone.ZoneIndex);
            }
        }

        private RevolverZone_SO SelectNormalZone(int currentRevolverZoneIndex)
        {
            foreach (RevolverZone_SO zone in normalZones)
            {
                if (zone.ZoneIndex == currentRevolverZoneIndex) return zone;
            }

            Debug.LogError($"No normal zone found for index {currentRevolverZoneIndex}");
            return null;
        }

        public static RevolverRules_SO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<RevolverRules_SO>("Data/Revolver Card Game/Revolver Rules/RevolverRules");
                    if (_instance == null)
                        Debug.LogError("RevolverRules_SO not found in Resources folder!");
                }
                return _instance;
            }
        }

    }
}