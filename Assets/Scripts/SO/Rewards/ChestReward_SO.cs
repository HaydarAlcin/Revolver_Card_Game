using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "New Chest Reward", menuName = "Revolver Card Game/Rewards/Chest Reward")]
    internal class ChestReward_SO : RevolverReward_SO
    {
        internal override void ClaimReward(int amount)
        {
            Debug.Log($"Claimed Chest Reward: {RewardType} - Amount: {amount}");
        }
    }
}
    
