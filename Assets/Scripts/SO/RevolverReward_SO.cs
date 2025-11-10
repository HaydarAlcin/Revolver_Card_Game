using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    internal class RevolverReward_SO : ScriptableObject
    {
        [SerializeField] private RewardType rewardType;
        [Space]
        [SerializeField] private int rewardAmount;
        [Space]
        [SerializeField] private Sprite rewardIcon;

        internal RewardType RewardType { get => rewardType; }
        internal Sprite RewardIcon { get => rewardIcon; }
        internal int Amount { get => rewardAmount; }

        internal virtual void ClaimReward(int amount) {}
    }
}