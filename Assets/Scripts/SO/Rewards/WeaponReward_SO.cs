using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "New Weapon Reward", menuName = "Revolver Card Game/Rewards/Weapon Reward")]
    internal class WeaponReward_SO : RevolverReward_SO
    {
        internal override void ClaimReward(int amount)
        {
            Debug.Log($"Claimed Weapon Reward: {RewardType} - Amount: {amount}");
        }
    }
}