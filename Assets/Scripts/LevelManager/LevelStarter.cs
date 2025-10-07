using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    [SerializeField] private Fort _startingFort;
    [SerializeField] private int _startingBotCount = 3;

    private void Start()
    {
        _startingFort.InitializeOnLevelStart(_startingBotCount);
    }
}
