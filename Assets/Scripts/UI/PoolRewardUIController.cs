using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.RevolverCardGame
{
    internal class PoolRewardUIController : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI rewardAmountText;

        private int _currentRewardAmount;
        private RevolverReward_SO _currentReward;

        internal RevolverReward_SO CurrentReward => _currentReward;
        
        internal int CurrentRewardAmount => _currentRewardAmount;

        internal void SetReward(RevolverReward_SO reward, Sprite icon, int amount)
        {
            _currentReward = reward;
            _currentRewardAmount = amount;
            rewardIcon.sprite = icon;
            rewardAmountText.SetText(_currentRewardAmount.ToK());
            this.gameObject.SetActive(true);
        }

        internal void UpdateRewardAmount(int amount)
        {
            _currentRewardAmount = amount;
            rewardAmountText.SetText(_currentRewardAmount.ToK());
        }

        internal void ResetPool()
        {
            _currentReward = null;
            _currentRewardAmount = 0;
            rewardIcon.sprite = null;
            rewardAmountText.SetText(string.Empty);
            this.gameObject.SetActive(false);
        }

        internal void ClaimReward() => _currentReward?.ClaimReward(_currentRewardAmount);
    }
}