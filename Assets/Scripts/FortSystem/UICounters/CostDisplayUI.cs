using TMPro;
using UnityEngine;

public class CostDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterText;

    public void UpdateCost(int count, int cost)
    {
        _counterText.text = count.ToString() + "/" + cost.ToString();
    }
}
