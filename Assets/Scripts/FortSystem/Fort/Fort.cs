using System.Collections;
using UnityEngine;

public class Fort : MonoBehaviour
{
    [SerializeField] private Database _database;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private FlagCreator _flagCreator;
    [SerializeField] private BotManager _botManager;
    [SerializeField] private PearlManager _pearlManager;

    private int _newBotCost = 3;
    private int _sendBotToFlagCost = 5;
    private bool _wasBotSendToFlag = false;

    private void OnDestroy()
    {
        _botManager.UnsubscribeAllBots();
        _scanner.PearlIsFound -= _database.OnPearlIsFound;
    }

    public void InitializeOnLevelStart(int botCount)
    {
        _botManager.Initialize(this, transform.position, transform.position);
        _botManager.SpawnBots(botCount);
        _scanner.PearlIsFound += _database.OnPearlIsFound;
        _pearlManager.ResetPearlsCount();
    }

    public void InitializeOnFlagCreate(Bot bot)
    {
        _botManager.Initialize(this, transform.position, transform.position);
        _botManager.ResetBots(bot);
        _scanner.PearlIsFound += _database.OnPearlIsFound;
        _pearlManager.ResetPearlsCount();
    }

    public void SpawnSingleBot()
    {
        int count = 1;
        _botManager.SpawnBots(count);
    }

    public void RemoveFlag()
    {
        _flagCreator.RemoveFlag();
    }

    public void OnBotReturned(Bot bot, Pearl pearl)
    {
        _database.RemoveAssignedPearl(pearl);
        pearl.NotifyReadyToReturn();
        _pearlManager.AddToPearlCount();
        ChoosePriorityToSpendResources(bot);
    }

    public void OnBotAvailable(Bot bot)
    {
        StartCoroutine(WaitForAvailablePearl(bot));
    }

    private void OnBotReachedFlag(Bot bot)
    {
        bot.ReachedFlag -= OnBotReachedFlag;
        _wasBotSendToFlag = false;
    }

    private void SendBotToFlag(Bot bot)
    {
        _botManager.PrepareBotToFlag(bot);
        bot.GoToFlag(_flagCreator.Flag, this);
        bot.ReachedFlag += OnBotReachedFlag;
        _wasBotSendToFlag = true;
    }

    private void ChoosePriorityToSpendResources(Bot bot)
    {
        if (_flagCreator.IsFlagOnMap && _wasBotSendToFlag == false && _botManager.BotsCount > 1)
        {
            if (_pearlManager.CanBuy(_sendBotToFlagCost))
            {
                SendBotToFlag(bot);
            }
        }
        else
        {
            if (_pearlManager.CanBuy(_newBotCost))
            {
                SpawnSingleBot();
            }
        }
    }

    private IEnumerator WaitForAvailablePearl(Bot bot)
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
}
