using TMPro;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _pearlCounter;

    public void UpdatePearlCounter(int pearlCount)
    {
        _pearlCounter.text = pearlCount.ToString();
    }
}
