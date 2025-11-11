using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace Game.Features.RevolverCardGame
{
    internal class RevolverUIController : MonoBehaviour
    {
        [Header("Reward UI Elements")]
        [SerializeField] private RewardUIController[] uIControllers;

        [Space(20)]
        [Header("Reward Pool Elements")]
        [SerializeField] private PoolRewardUIController[] poolRewardControllers;

        [Space(20)]
        [Header("Revolver Images")]
        [SerializeField] private Image revolverSpinImage;
        [SerializeField] private Image revolverTriggerImage;

        [Space(20)]
        [Header("Spin")]
        [SerializeField] private RevolverSpinConfig_SO spinConfig;
        [SerializeField] private RectTransform spinViewContent;
        [SerializeField] private RectTransform[] spinTargets;

        [Space(20)]
        [Header("Game Over Panel")]
        [SerializeField] private CanvasGroup gameOverPanel;

        [Space(20)]
        [Header("Zone Txts")]
        [SerializeField] private TextMeshProUGUI currentZoneIndexText;
        [SerializeField] private TextMeshProUGUI nextSafeZoneIndexText;
        [SerializeField] private TextMeshProUGUI nextGoldZoneIndexTxt;

        [Space(20)]
        [Header("Buttons")]
        [SerializeField] private Button addCoinButton;
        [SerializeField] private Button spinButton;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button claimButton;

        // Pools
        private readonly List<PoolRewardUIController> _activePools = new(1);
        private readonly List<PoolRewardUIController> _inactivePools = new(1);

        // Game State
        private RevolverReward_SO _targetReward;
        private bool _isGameOver,_isGameStart;
        private int _targetRewardIndex;

        // Scroll Settings
        private Tween _spinTween, _gameOverPanelTween;
        private Ease _easeType;
        private int _spinDuration, _spinLoops;
        private float _afterSpinDelay;

        private bool _clockwise, _snapping;

        private float _animZ, _prevZ;
        private float _defaultTopAngle;
        private float[] _crossAngles;

        private void Start()
        {
            InitializeSpinSettings();
            InitializeButtons();
            ResetPoolElements();
            PrepareCrossings();
        }

        private void OnDisable() => ResetPanel();

        private void InitializeSpinSettings()
        {
            _easeType = spinConfig.SpinEase;
            _clockwise = spinConfig.Clockwise;
            _snapping = spinConfig.Snapping;
            _defaultTopAngle = spinConfig.TopAngle;
            _afterSpinDelay = spinConfig.AfterSpinDelay;

            ResetSpinDurationAndLoops();
        }

        private void PrepareCrossings()
        {
            if (!spinViewContent || spinTargets == null || spinTargets.Length == 0) return;

            _crossAngles = new float[spinTargets.Length];
            for (int i = 0; i < spinTargets.Length; i++)
            {
                var marker = GetMarker(spinTargets[i]);
                if (!marker) { _crossAngles[i] = float.NaN; continue; }

                Vector2 lp = spinViewContent.InverseTransformPoint(marker.position);
                float angLocal = Mathf.Atan2(lp.y, lp.x) * Mathf.Rad2Deg;

                _crossAngles[i] = (_defaultTopAngle - angLocal).Normalize360();
            }
        }

        private void InitializeButtons()
        {
            addCoinButton.onClick.AddListener(OnAddCoinButtonPressed);
            spinButton.onClick.AddListener(OnSpinButtonPressed);
            giveUpButton.onClick.AddListener(OnGiveUpButtonPressed);
            reviveButton.onClick.AddListener(OnReviveButtonPressed);
            claimButton.onClick.AddListener(OnClaimButtonPressed);
        }

        private void ResetPanel()
        {
            _spinTween?.Kill();
            _targetReward = null;
            _spinTween = null;

            addCoinButton.onClick.RemoveAllListeners();
            spinButton.onClick.RemoveAllListeners();
            giveUpButton.onClick.RemoveAllListeners();
            reviveButton.onClick.RemoveAllListeners();
            claimButton.onClick.RemoveAllListeners();
        }

        internal void SetupRevolverUI(List<RevolverReward_SO> rewards, int targetRewardIndex, bool isGameOver)
        {
            _isGameOver = isGameOver;
            _targetRewardIndex = targetRewardIndex;
            _targetReward = _targetRewardIndex >= rewards.Count ? null : rewards[targetRewardIndex];

            uIControllers[^1]?.SetupDeathSprite(spinConfig.DeathSprite);

            for (int i = 0; i < rewards.Count; i++)
                uIControllers[i]?.SetupRewardUI(rewards[i]);

            ResetSpinDurationAndLoops();
        }


        internal void SetCurrentZoneUIObjects(int currentIndex,Sprite spinIcon, Sprite triggerIcon)
        {
            if(revolverSpinImage.sprite != spinIcon)
                revolverSpinImage.sprite = spinIcon;
            if (revolverTriggerImage.sprite != triggerIcon)
                revolverTriggerImage.sprite = triggerIcon;

            currentZoneIndexText.SetText((currentIndex + 1).ToString());

            int nextSafeZoneIndex = RevolverRules_SO.Instance.GetNextSafeZoneIndex(currentIndex) + 1;
            int nextGoldZoneIndex = RevolverRules_SO.Instance.GetNextGoldZoneIndex(currentIndex) + 1;

            nextSafeZoneIndexText.SetText(nextSafeZoneIndex.ToString());
            nextGoldZoneIndexTxt.SetText(nextGoldZoneIndex.ToString());
        }

        private void OnSpinRevolver(int index)
        {
            if (!spinViewContent || spinTargets == null || spinTargets.Length == 0) return;
            index = Mathf.Clamp(index, 0, spinTargets.Length - 1);

            var marker = GetMarker(spinTargets[index]);
            Vector2 lp = spinViewContent.InverseTransformPoint(marker.position);
            float targetLocal = Mathf.Atan2(lp.y, lp.x) * Mathf.Rad2Deg;

            float currentZ = _animZ = spinViewContent.localEulerAngles.z;
            float childWorldNow = currentZ + targetLocal;
            float baseDelta = Mathf.DeltaAngle(childWorldNow, _defaultTopAngle);

            if (_clockwise && baseDelta > 0f) baseDelta -= 360f;
            else if (!_clockwise && baseDelta < 0f) baseDelta += 360f;

            float finalZ = currentZ + baseDelta + (_clockwise ? -360f : 360f) * Mathf.Max(0, _spinLoops);


            float degPerSec = Mathf.Abs(finalZ - currentZ) / Mathf.Max(0.0001f, _spinDuration);
            float sep = 360f / Mathf.Max(1, spinTargets.Length);

            if (_snapping) finalZ = Mathf.Round(finalZ);

            _prevZ = _animZ;
            _spinTween?.Kill();
            _spinTween = DOTween
                .To(
                    () => _animZ,
                    z => 
                    {
                        _prevZ = _animZ; _animZ = z;
                        spinViewContent.localRotation = Quaternion.Euler(0f, 0f, _animZ);
                        CheckCrosses(_prevZ, _animZ);
                    },
                    finalZ,
                    _spinDuration
                )
                .SetEase(_easeType)
                .OnComplete(() => DOVirtual.DelayedCall(_afterSpinDelay, OnSpinComplete, ignoreTimeScale: true));
        }

        public void OnAddCoinButtonPressed()
            => GameManager.Instance.AddGold(RevolverRules_SO.Instance.RevolverGoldPrice);

        public void OnSpinButtonPressed()
        {
            if (!_isGameStart)
            {
                int tmpPrice = RevolverRules_SO.Instance.RevolverGoldPrice;
                bool canScroll = GameManager.Instance.CheckGold(tmpPrice);
                if (!canScroll) return;

                _isGameStart = true;
                GameManager.Instance.AddGold(-tmpPrice);
            }

            spinButton.interactable = false;
            claimButton.interactable = false;
            OnSpinRevolver(_targetRewardIndex);
        }

        private void OnGiveUpButtonPressed()
        {
            FadeGameOverPanel(false);
            ResetPoolElements();
            EventManager.CoreRevolverGameSignals.OnRevolverGameEnded?.Invoke();
        }

        private void OnReviveButtonPressed()
        {
            int tmpRevivePrice = RevolverRules_SO.Instance.ReviveGoldPrice;
            bool canRevive = GameManager.Instance.CheckGold(tmpRevivePrice);
            
            if (!canRevive) return;
            _isGameStart = true;
            FadeGameOverPanel(false);
            GameManager.Instance.AddGold(-tmpRevivePrice);
            EventManager.CoreRevolverGameSignals.OnRevolverGameRevived?.Invoke();
        }

        private void OnClaimButtonPressed()
        {
            if (!_isGameStart) return;
            _isGameStart = false;
            ClaimPoolRewards();
            ResetPoolElements();
            EventManager.CoreRevolverGameSignals.OnRevolverGameEnded?.Invoke();
        }

        private void OnSpinComplete()
        {
            if (_isGameOver)
            {
                _isGameStart = false;
                FadeGameOverPanel(true);
            }
            else
                DepositRewardToPool();

            spinButton.interactable = true;
            claimButton.interactable = true;
        }

        private void CheckCrosses(float z0, float z1)
        {
            if (_crossAngles == null) return;

            float min = Mathf.Min(z0, z1);
            float max = Mathf.Max(z0, z1);

            for (int i = 0; i < _crossAngles.Length; i++)
            {
                float thr = _crossAngles[i];
                if (float.IsNaN(thr)) continue;

                float shifted = thr + 360f * Mathf.Ceil((min - thr) / 360f);
                if (shifted > min && shifted <= max)
                    PlayTick();
            }
        }

        private void PlayTick()
            => EventManager.CoreAudioSignals.OnPlayRevolverTriggerSFX?.Invoke();

        private RectTransform GetMarker(RectTransform t)
        {
            if (t && t.childCount > 0) return t.GetChild(0) as RectTransform;
            return t;
        }

        private void ResetSpinDurationAndLoops()
        {
            _spinDuration = spinConfig.GetRandomDuration();
            _spinLoops = spinConfig.GetRandomLoops();
        }

        private void FadeGameOverPanel(bool visible)
        {
            float target = visible ? 1f : 0f;

            gameOverPanel.gameObject.SetActive(visible);

            _gameOverPanelTween?.Kill();
            _gameOverPanelTween = gameOverPanel
                .DOFade(target, 0.5f)
                .SetEase(Ease.Linear);

            gameOverPanel.interactable = visible;
            gameOverPanel.blocksRaycasts = visible;
        }

        private void ClaimPoolRewards()
        {
            foreach (var pool in _activePools)
                pool.ClaimReward();
        }

        private void ResetPoolElements()
        {
            foreach (var pool in _activePools)
                pool.ResetPool();

            _activePools.Clear();
            _inactivePools.Clear();
            _inactivePools.AddRange(poolRewardControllers);
        }

        private void DepositRewardToPool()
        {
            bool activePoolFound = false;
            foreach (var pool in _activePools)
            {
                if (pool.CurrentReward.RewardType == _targetReward.RewardType)
                {
                    int newAmount = pool.CurrentRewardAmount + _targetReward.Amount;
                    pool.UpdateRewardAmount(newAmount);
                    activePoolFound = true;
                    break;
                }
            }

            if (!activePoolFound && _inactivePools.Count > 0)
            {
                var pool = _inactivePools[0];
                pool.SetReward(_targetReward, _targetReward.RewardIcon, _targetReward.Amount);
                _activePools.Add(pool);
                _inactivePools.RemoveAt(0);
            }

            EventManager.CoreRevolverGameSignals.OnSpinCompleted?.Invoke();
        }
    }
}