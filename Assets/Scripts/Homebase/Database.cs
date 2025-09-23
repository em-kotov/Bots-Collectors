using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;

    private Queue<Pearl> _scannedPearls = new Queue<Pearl>();
    private HashSet<Pearl> _assignedPearls = new HashSet<Pearl>();

    private void OnEnable()
    {
        _scaner.PearlIsFound += OnPearlIsFound;
    }

    private void OnDisable()
    {
        _scaner.PearlIsFound -= OnPearlIsFound;
    }

    public void RemoveAssignedPearl(Pearl pearl)
    {
        _assignedPearls.Remove(pearl);
    }

    public bool TryGetScannedPearl(out Pearl pearl)
    {
        bool isAvailable = false;
        pearl = null;

        if (_scannedPearls.Count > 0)
        {
            pearl = _scannedPearls.Dequeue();
            _assignedPearls.Add(pearl);
            isAvailable = true;
        }

        return isAvailable;
    }

    private void OnPearlIsFound(Pearl pearl)
    {
        if (_scannedPearls.Contains(pearl) || _assignedPearls.Contains(pearl))
        {
            return;
        }

        _scannedPearls.Enqueue(pearl);
    }
}
