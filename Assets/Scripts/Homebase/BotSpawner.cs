using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _botCount = 3;
    [SerializeField] private float _slotRadius = 3f;

    public List<Bot> GetBots(int count, Vector3 center, float height)
    {
        List<Bot> bots = new List<Bot>(count);

        List<Vector3> slotPositions = Vector3Extensions.GetPositionsInRadius(center, _slotRadius, height, _botCount);

        for (int i = 0; i < count; i++)
        {
            Bot bot = Instantiate(_botPrefab);
            bots.Add(bot);
            Vector3 slotPosition = slotPositions[i];
            bot.SetBasePosition(slotPosition);
        }

        return bots;
    }
}
