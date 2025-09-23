using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4.8f;

    private float _distanceChecker = 0.1f;
    private Coroutine _moveRoutine;

    public void MoveToTarget(Transform bot, Vector3 target)
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        _moveRoutine = StartCoroutine(MoveToPosition(bot, target));
    }

    public bool IsEnoughClose(Vector3 bot, Vector3 target)
    {
        return bot.IsEnoughClose(target, _distanceChecker);
    }

    private IEnumerator MoveToPosition(Transform bot, Vector3 targetPosition)
    {
        while (bot.position.IsEnoughClose(targetPosition, _distanceChecker) == false)
        {
            bot.position = Vector3.MoveTowards(bot.position, targetPosition, _moveSpeed * Time.deltaTime);
            bot.LookAt(targetPosition);
            yield return null;
        }

        _moveRoutine = null;
    }
}
