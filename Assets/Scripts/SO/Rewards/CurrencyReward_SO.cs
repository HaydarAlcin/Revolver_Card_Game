using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "New Currency Reward", menuName = "Revolver Card Game/Rewards/Currency Reward")]
    internal class CurrencyReward_SO : RevolverReward_SO
    {
        internal override void ClaimReward(int amount)
        {
            switch (RewardType)
            {
                case RewardType.Gold:
                    GameManager.Instance.AddGold(amount);
                    break;
                case RewardType.Cash:
                    GameManager.Instance.AddCash(amount);
                    break;
            }

            Debug.Log($"Claimed Currency Reward: {RewardType} - Amount: {amount}");
        }
    }
}