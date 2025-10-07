using UnityEngine;

public class PearlManager : MonoBehaviour
{
    [SerializeField] private PearlCounterUI _pearlCounter;
    [SerializeField] private CostDisplayUI _costDisplay;

    private int _totalPearlsCount = 0;
    private int _pearlsToSpend = 0;
    private int _resetCost = 0;

    public void AddToPearlCount()
    {
        _pearlsToSpend++;
        _totalPearlsCount++;
        UpdateCounter();
    }

    public void ResetPearlsCount()
    {
        _pearlsToSpend = 0;
        _totalPearlsCount = 0;
        UpdateCounter();
        ShowCost(_resetCost);
    }

    public bool CanBuy(int cost)
    {
        ShowCost(cost);

        if (_pearlsToSpend >= cost)
        {
            _pearlsToSpend = 0;
            return true;
        }

        return false;
    }

    private void UpdateCounter()
    {
        _pearlCounter.UpdateCounter(_totalPearlsCount);
    }

    private void ShowCost(int cost)
    {
        _costDisplay.UpdateCost(_pearlsToSpend, cost);
    }
}
