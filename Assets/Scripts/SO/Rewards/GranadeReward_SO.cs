using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "New Granade Reward", menuName = "Revolver Card Game/Rewards/Granade Reward")]
    internal class GranadeReward_SO : RevolverReward_SO
    {
        internal override void ClaimReward(int amount)
        {
            Debug.Log($"Claimed Granade Reward: {RewardType} - Amount: {amount}");
        }
    }
}