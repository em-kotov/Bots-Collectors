using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homebase : MonoBehaviour
{
    [SerializeField] private Database _database;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private CounterUI _counter;
    [SerializeField] private int _botCount = 3;

    private List<Bot> _bots = new List<Bot>();
    private int _collectedPearlsCount = 0;

    private void Start()
    {
        _bots = _botSpawner.GetBots(_botCount, transform.position, transform.position.y);

        for (int i = 0; i < _bots.Count; i++)
        {
            InitializeBot(_bots[i]);
        }

        _counter.UpdatePearlCounter(_collectedPearlsCount);
    }

    private void OnDestroy()
    {
        foreach (Bot bot in _bots)
        {
            if (bot != null)
            {
                bot.HaveReturned -= OnBotReturned;
                bot.BecomeAvailable -= OnBotAvailable;
            }
        }
    }

    private void InitializeBot(Bot bot)
    {
        bot.HaveReturned += OnBotReturned;
        bot.BecomeAvailable += OnBotAvailable;
        bot.Initialize();
    }

    private IEnumerator SendBot(Bot bot)
    {
        float tryDelay = 1f;
        WaitForSeconds wait = new WaitForSeconds(tryDelay);
        Pearl targetPearl = null;

        while (targetPearl == null)
        {
            if (_database.TryGetScannedPearl(out targetPearl))
            {
                break;
            }

            yield return wait;
        }

        bot.Collect(targetPearl);
    }

    private void OnBotReturned(Bot bot, Pearl pearl)
    {
        _database.RemoveAssignedPearl(pearl);
        _collectedPearlsCount++;
        _counter.UpdatePearlCounter(_collectedPearlsCount);
        pearl.NotifyReadyToReturn();
    }

    private void OnBotAvailable(Bot bot)
    {
        StartCoroutine(SendBot(bot));
    }
}
