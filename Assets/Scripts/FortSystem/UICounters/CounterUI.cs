using TMPro;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterText;

    public void UpdateCounter(int count)
    {
        _counterText.text = count.ToString();
    }
}
