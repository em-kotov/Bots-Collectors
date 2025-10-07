using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4.8f;

    private float _distanceChecker = 0.1f;
    private Coroutine _moveRoutine;
    private Transform _botTransform;

    public bool HasReachedTarget => _moveRoutine == null;

    public void SetBotTransform(Transform transform)
    {
        _botTransform = transform;
    }

    public void GoToTransform(Transform target)
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        _moveRoutine = StartCoroutine(MoveToTransform(target));
    }

    public void GoToPosition(Vector3 target)
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        _moveRoutine = StartCoroutine(MoveToPosition(target));
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        while (_botTransform.position.IsEnoughClose(target, _distanceChecker) == false)
        {
            _botTransform.position = Vector3.MoveTowards(_botTransform.position, target, _moveSpeed * Time.deltaTime);
            _botTransform.LookAt(target);
            yield return null;
        }

        _moveRoutine = null;
    }

    private IEnumerator MoveToTransform(Transform target)
    {
        while (_botTransform.position.IsEnoughClose(target.position, _distanceChecker) == false)
        {
            _botTransform.position = Vector3.MoveTowards(_botTransform.position, target.position, _moveSpeed * Time.deltaTime);
            _botTransform.LookAt(target);
            yield return null;
        }

        _moveRoutine = null;
    }
}
