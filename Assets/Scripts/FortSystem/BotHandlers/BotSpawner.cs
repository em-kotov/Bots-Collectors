using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private float _parkingPositionsRadius = 4f;

    public List<Bot> SpawnBots(int count)
    {
        List<Bot> bots = new List<Bot>(count);

        for (int i = 0; i < count; i++)
        {
            Bot bot = Instantiate(_botPrefab, transform.position, Quaternion.identity);
            bot.Initialize();
            bots.Add(bot);
        }

        return bots;
    }

    public void RefreshPositions(List<Bot> bots, Vector3 center, float height)
    {
        List<Vector3> parkingPositions = Vector3Extensions.GetPositionsInRadius(center, _parkingPositionsRadius, height, bots.Count);

        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].SetParkingPosition(parkingPositions[i]);
        }
    }
}
