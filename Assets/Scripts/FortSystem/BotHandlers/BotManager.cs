using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private BotCounterUI _botCounter;

    private List<Bot> _bots = new List<Bot>();
    private Vector3 _pearlStoragePosition;
    private Vector3 _parkingCenter;
    private float _parkingHeight;
    private Fort _fort;

    public int BotsCount => _bots.Count;

    public void Initialize(Fort fort, Vector3 pearlStoragePosition, Vector3 parkingCenter)
    {
        _fort = fort;
        _pearlStoragePosition = pearlStoragePosition;
        _parkingCenter = parkingCenter;
        _parkingHeight = _parkingCenter.y;
    }

    public void UnsubscribeAllBots()
    {
        foreach (Bot bot in _bots)
        {
            if (bot != null)
            {
                UnsubscribeBot(bot);
            }
        }
    }

    public void SpawnBots(int botCount)
    {
        List<Bot> bots = _botSpawner.SpawnBots(botCount);

        for (int i = 0; i < bots.Count; i++)
        {
            AddBot(bots[i]);
        }

        UpdateCounter();
        UpdateParkingPositions();
    }

    public void ResetBots(Bot bot)
    {
        _bots = new List<Bot>();
        AddBot(bot);
        UpdateCounter();
        UpdateParkingPositions();
    }

    public void PrepareBotToFlag(Bot bot)
    {
        UnsubscribeBot(bot);
        _bots.Remove(bot);
        UpdateCounter();
        UpdateParkingPositions();
    }

    private void AddBot(Bot bot)
    {
        bot.HaveReturned += _fort.OnBotReturned;
        bot.BecomeAvailable += _fort.OnBotAvailable;
        bot.AssignToFort(_pearlStoragePosition);
        _bots.Add(bot);
    }

    private void UnsubscribeBot(Bot bot)
    {
        bot.HaveReturned -= _fort.OnBotReturned;
        bot.BecomeAvailable -= _fort.OnBotAvailable;
    }

    private void UpdateCounter()
    {
        _botCounter.UpdateCounter(_bots.Count);
    }

    private void UpdateParkingPositions()
    {
        _botSpawner.RefreshPositions(_bots, _parkingCenter, _parkingHeight);
    }
}
