using UnityEngine;
using System.Collections.Generic;

namespace Game.Features.RevolverCardGame
{
    internal class RevolverGameManager : MonoBehaviour
    {
        [Header("Revolver UI Controller")]
        [SerializeField] private RevolverUIController revolverUIController;

        private RevolverRules_SO _revolverRulesData;

        private int _currentRevolverZoneIndex;
        private int _gameOverIndex;

        bool _isGameOver;

        private void OnEnable()
        {
            EventManager.CoreRevolverGameSignals.OnSpinCompleted += OnSpinCompleted;
            EventManager.CoreRevolverGameSignals.OnRevolverGameEnded += OnRevolverGameEnded;
            EventManager.CoreRevolverGameSignals.OnRevolverGameRevived += OnRevolverGameRevived;
        }

        private void OnDisable()
        {
            EventManager.CoreRevolverGameSignals.OnSpinCompleted -= OnSpinCompleted;
            EventManager.CoreRevolverGameSignals.OnRevolverGameEnded -= OnRevolverGameEnded;
            EventManager.CoreRevolverGameSignals.OnRevolverGameRevived -= OnRevolverGameRevived;
        }

        private void Start()
        {
            _revolverRulesData = RevolverRules_SO.Instance;
            _revolverRulesData.InitializeZoneDictionaries();
            InitializeRevolverGame();
        }

        private void InitializeRevolverGame()
        {
            _gameOverIndex = _revolverRulesData.PickGameOverIndex();
            _currentRevolverZoneIndex = 0;
            _isGameOver = false;
            SetupRevolverRewards();

            Debug.Log(_gameOverIndex);
        }

        private void SetupRevolverRewards()
        {
            RevolverZone_SO rewardZone = _revolverRulesData.SelectRevolverZone(_currentRevolverZoneIndex);
            List<RevolverReward_SO> rewards = rewardZone.GetRandomRewards(_revolverRulesData.GetMaxRewardPerZone(_currentRevolverZoneIndex));
            
            int targetRewardIndex = _isGameOver ? _revolverRulesData.MaxRewardsPerSection - 1 : Random.Range(0,rewards.Count);
            revolverUIController?.SetupRevolverUI(rewards, targetRewardIndex, _isGameOver);
            revolverUIController?.SetCurrentZoneUIObjects(_currentRevolverZoneIndex,rewardZone.SpinIcon, rewardZone.TriggerIcon);
        }

        private void OnSpinCompleted()
        {
            _currentRevolverZoneIndex++;
            if (_currentRevolverZoneIndex == _gameOverIndex) _isGameOver = true;
            SetupRevolverRewards();
        }

        private void OnRevolverGameEnded() => InitializeRevolverGame();

        private void OnRevolverGameRevived()
        {
            _gameOverIndex = _revolverRulesData.PickNextIndexWithin3(_gameOverIndex);
            _isGameOver = false;
            SetupRevolverRewards();
        }
    }
}