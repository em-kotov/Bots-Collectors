using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlSpawner : Spawner<Pearl>
{
    [SerializeField] private float _chance = 0.5f;
    [SerializeField] private float _spawnRate = 1.5f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _height = 2f;
    [SerializeField] private int _maxCount = 20;

    private Coroutine _spawnRoutine;
    private HashSet<Pearl> _activePearls = new HashSet<Pearl>();

    private void OnEnable()
    {
        if (_spawnRoutine == null)
        {
            _spawnRoutine = StartCoroutine(SpawnPearls());
        }
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    protected override void OnGet(Pearl item)
    {
        Vector3 randomOffset = Random.insideUnitSphere * _radius;
        Vector3 spawnPosition = transform.position + randomOffset;
        spawnPosition.y = _height;

        base.OnGet(item);
        SetItemPosition(item, spawnPosition);
        item.Initialize();
        item.PickedUp += OnPearlPickedUp;
        item.ReadyToReturn += OnReadyToReturn;
        _activePearls.Add(item);
    }

    public void SpawnSinglePearl()
    {
        Pool.Get();
    }

    private IEnumerator SpawnPearls()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnRate);

        while (isActiveAndEnabled)
        {
            yield return wait;

            if (_activePearls.Count < _maxCount && Random.value <= _chance)
            {
                SpawnSinglePearl();
            }
        }
    }

    private void OnReadyToReturn(Pearl pearl)
    {
        pearl.ReadyToReturn -= OnReadyToReturn;
        Pool.Release(pearl);
    }

    private void OnPearlPickedUp(Pearl pearl)
    {
        _activePearls.Remove(pearl);
        pearl.PickedUp -= OnPearlPickedUp;
    }
}
