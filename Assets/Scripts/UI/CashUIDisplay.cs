using TMPro;
using UnityEngine;

public class CashUIDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;

    private void Start()
        => EventManager.CoreUISignals.OnTotalCashChanged += OnTotalCashChanged;

    private void OnDestroy()
        => EventManager.CoreUISignals.OnTotalCashChanged -= OnTotalCashChanged;

    private void OnTotalCashChanged(int totalCash)
        => cashText.SetText(totalCash.ToString());
}