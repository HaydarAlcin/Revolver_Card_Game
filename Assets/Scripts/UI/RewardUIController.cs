using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.RevolverCardGame
{
    internal class RewardUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI rewardAmountText;


        internal void SetupRewardUI(RevolverReward_SO revolverReward)
        {
            rewardIcon.sprite = revolverReward.RewardIcon;
            rewardAmountText.SetText(revolverReward.Amount.ToK());
        }

        internal void SetupDeathSprite(Sprite deathSprite)
        {
            rewardIcon.sprite = deathSprite;
            rewardAmountText.SetText(string.Empty);
        }
    }
}