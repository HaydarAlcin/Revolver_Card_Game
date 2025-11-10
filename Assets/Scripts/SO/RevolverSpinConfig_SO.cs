using DG.Tweening;
using UnityEngine;

namespace Game.Features.RevolverCardGame
{
    [CreateAssetMenu(fileName = "RevolverSpinConfig_SO", menuName = "Revolver Card Game/Revolver Spin Config/RevolverSpinConfig_SO")]
    public class RevolverSpinConfig_SO : ScriptableObject
    {
        public Ease SpinEase = Ease.OutCubic;

        [Header("Bomb Sprite")]
        public Sprite DeathSprite;

        [Header("Duration")]
        public float AfterSpinDelay;
        public int MinSpinDuration;
        public int MaxSpinDuration;

        [Header("Loops")]
        public int MinSpinLoops;
        public int MaxSpinLoops;

        [Header("Spin Settings")]
        public float TopAngle = 90f;
        public bool Clockwise = true;
        public bool Snapping = true;

        public int GetRandomDuration()
        {
            return Random.Range(MinSpinDuration, MaxSpinDuration + 1);
        }
        public int GetRandomLoops()
        {
            return Random.Range(MinSpinLoops, MaxSpinLoops + 1);
        }
    }
}