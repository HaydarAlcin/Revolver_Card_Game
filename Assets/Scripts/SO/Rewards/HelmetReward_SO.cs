using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "New Helmet Reward", menuName = "Revolver Card Game/Rewards/Helmet Reward")]
    internal class HelmetReward_SO : RevolverReward_SO
    {
        internal override void ClaimReward(int amount)
        {
            Debug.Log($"Claimed Helmet Reward: {RewardType} - Amount: {amount}");
        }
    }
}