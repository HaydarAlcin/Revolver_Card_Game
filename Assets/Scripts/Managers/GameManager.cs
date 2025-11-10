using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _totalGold;
    private int _totalCash;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Application.targetFrameRate = 60;
    }

    public bool CheckGold(int price)
    {
        return _totalGold >= price;
    }

    public void AddGold(int v)
    {
        _totalGold += v;
        EventManager.CoreUISignals.OnTotalGoldChanged?.Invoke(_totalGold);
    }

    public void AddCash(int v)
    {
        _totalCash += v;
        EventManager.CoreUISignals.OnTotalCashChanged?.Invoke(_totalCash);
    }
}