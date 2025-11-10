using UnityEngine;
using System.Collections.Generic;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "RevolverZone_SO", menuName = "Revolver Card Game/Revolver Zone/RevolverZone_SO")]
    public class RevolverZone_SO : ScriptableObject
    {
        [SerializeField] private int zoneIndex;
        [SerializeField] private Sprite spinIcon;
        [SerializeField] private Sprite triggerIcon;

        [Space(20)]
        [SerializeField] private RevolverReward_SO[] rewards;

        internal int ZoneIndex => zoneIndex;
        internal Sprite SpinIcon => spinIcon;
        internal Sprite TriggerIcon => triggerIcon;


        internal List<RevolverReward_SO> GetRandomRewards(int maxCount)
        {
            var count = Mathf.Min(maxCount, rewards.Length);

            List<RevolverReward_SO> selectedRewards = new List<RevolverReward_SO>();
            List<int> usedIndices = new List<int>();
            while (selectedRewards.Count < count)
            {
                int randomIndex = Random.Range(0, rewards.Length);
                if (!usedIndices.Contains(randomIndex))
                {
                    usedIndices.Add(randomIndex);
                    selectedRewards.Add(rewards[randomIndex]);
                }
            }

            return selectedRewards;
        }
    }
}