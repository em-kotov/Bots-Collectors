using System.Collections;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 40f;
    [SerializeField] private float _scanRate = 1f;
    [SerializeField] private ScanerEffects _effects;

    private Coroutine _scanRoutine;

    public System.Action<Pearl> PearlIsFound;

    private void OnEnable()
    {
        if (_scanRoutine == null)
        {
            _scanRoutine = StartCoroutine(ScanForPearls());
        }
    }

    private void Start()
    {
        _effects.SetScanRadius(_scanRadius, transform.position.y);
    }

    private void OnDisable()
    {
        if (_scanRoutine != null)
        {
            StopCoroutine(_scanRoutine);
            _scanRoutine = null;
        }
    }

    private IEnumerator ScanForPearls()
    {
        WaitForSeconds wait = new WaitForSeconds(_scanRate);

        while (isActiveAndEnabled)
        {
            yield return wait;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _scanRadius);
            _effects.PlayScanEffect();

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent(out Pearl pearl) == false)
                {
                    continue;
                }

                PearlIsFound?.Invoke(pearl);
            }
        }
    }
}
