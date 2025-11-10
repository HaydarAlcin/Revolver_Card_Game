using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "New Healthshot Reward", menuName = "Revolver Card Game/Rewards/Healthshot Reward")]
    internal class HealthshotReward_SO : RevolverReward_SO
    {
        internal override void ClaimReward(int amount)
        {
            Debug.Log($"Claimed Healthshot Reward: {RewardType} - Amount: {amount}");
        }
    }
}