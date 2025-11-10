using TMPro;
using UnityEngine;

public class GoldUIDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
        => EventManager.CoreUISignals.OnTotalGoldChanged += OnTotalGoldChanged;

    private void OnDestroy()
        => EventManager.CoreUISignals.OnTotalGoldChanged -= OnTotalGoldChanged;

    private void OnTotalGoldChanged(int totalGold)
        => goldText.SetText(totalGold.ToString());
}