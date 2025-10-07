using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    private Queue<Pearl> _scannedPearls = new Queue<Pearl>();
    private HashSet<Pearl> _assignedPearls = new HashSet<Pearl>();

    public void RemoveAssignedPearl(Pearl pearl)
    {
        _assignedPearls.Remove(pearl);
    }

    public bool TryGetScannedPearl(out Pearl pearl)
    {
        pearl = null;

        while (_scannedPearls.Count > 0)
        {
            pearl = _scannedPearls.Dequeue();

            if (_assignedPearls.Contains(pearl))
            {
                continue;
            }

            _assignedPearls.Add(pearl);
            return true;
        }

        return false;
    }

    public void OnPearlIsFound(Pearl pearl)
    {
        if (_scannedPearls.Contains(pearl) || _assignedPearls.Contains(pearl))
        {
            return;
        }

        _scannedPearls.Enqueue(pearl);
    }
}
