using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Homebase : MonoBehaviour
{
    [Header("Scan Settings")]
    [SerializeField] private float _scanRadius = 8f;
    [SerializeField] private float _scanRate = 1f;
    [SerializeField] private HomebaseEffects _effects;

    [Header("Bot Settings")]
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _botCount = 3;
    [SerializeField] private float _slotRadius = 3f;

    private List<Bot> _bots = new List<Bot>();
    private HashSet<Pearl> _collectedPearls = new HashSet<Pearl>();
    private Queue<Pearl> _scannedPearls = new Queue<Pearl>();
    private HashSet<Pearl> _assignedPearls = new HashSet<Pearl>();
    private Coroutine _scanRoutine;

    private void Start()
    {
        List<Vector3> slotPositions = Vector3Extensions.GetPositionsInRadius(transform.position, _slotRadius, transform.position.y, _botCount);
        _bots = new List<Bot>(_botCount);

        for (int i = 0; i < _botCount; i++)
        {
            Bot bot = Instantiate(_botPrefab);
            _bots.Add(bot);
            Vector3 slotPosition = slotPositions[i];
            bot.Initialize(slotPosition);
            bot.HaveReturned += OnBotReturned;
        }

        _effects.SetScanRadius(_scanRadius, transform.position.y);
        _effects.UpdatePearlCounter(_collectedPearls.Count);
        _scanRoutine = StartCoroutine(ScanForPearls());
    }

    private void OnEnable()
    {
        if (_scanRoutine == null)
        {
            _scanRoutine = StartCoroutine(ScanForPearls());
        }
    }

    private void OnDisable()
    {
        if (_scanRoutine != null)
        {
            StopCoroutine(_scanRoutine);
            _scanRoutine = null;
        }
    }

    private void OnDestroy()
    {
        foreach (Bot bot in _bots)
        {
            if (bot != null)
            {
                bot.HaveReturned -= OnBotReturned;
            }
        }
    }

    private IEnumerator ScanForPearls()
    {
        WaitForSeconds wait = new WaitForSeconds(_scanRate);

        while (isActiveAndEnabled)
        {
            yield return wait;

            TrySendBot();

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _scanRadius);
            _effects.PlayScanEffect();

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent(out Pearl pearl) == false ||
                    _assignedPearls.Contains(pearl) || _scannedPearls.Contains(pearl))
                {
                    continue;
                }

                _scannedPearls.Enqueue(pearl);
            }
        }
    }

    private void TrySendBot()
    {
        while (_scannedPearls.Count > 0)
        {
            Bot availableBot = _bots.FirstOrDefault(bot => bot.IsAvailable);

            if (availableBot == null)
            {
                break;
            }

            Pearl pearl = _scannedPearls.Dequeue();
            _assignedPearls.Add(pearl);
            availableBot.Collect(pearl.transform.position);
        }
    }

    private void OnBotReturned(Bot bot, Pearl pearl)
    {
        _assignedPearls.Remove(pearl);
        _collectedPearls.Add(pearl);
        _effects.UpdatePearlCounter(_collectedPearls.Count);
    }
}
