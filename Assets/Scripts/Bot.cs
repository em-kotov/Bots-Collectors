using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
   [SerializeField] private Transform _holdPoint;
   [SerializeField] private float _moveSpeed = 4.8f;
   [SerializeField] private float _scanRadius = 0.5f;
   [SerializeField] private BotEffects _effects;

   private Pearl _pearlSlot;
   private Vector3 _basePosition;

   public bool IsAvailable { get; private set; } = true;

   public UnityAction<Bot, Pearl> HaveReturned;

   public void Initialize(Vector3 basePosition)
   {
      transform.position = basePosition;
      _basePosition = basePosition;

      _effects.OverrideMaterialColor(GetComponentsInChildren<Renderer>());
   }

   public void Collect(Vector3 pearlPosition)
   {
      IsAvailable = false;
      StartCoroutine(CollectRoutine(pearlPosition));
   }

   private IEnumerator CollectRoutine(Vector3 pearlPosition)
   {
      yield return StartCoroutine(MoveToPosition(pearlPosition));

      Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius);

      foreach (Collider collider in colliders)
      {
         if (collider.gameObject.TryGetComponent(out Pearl pearl))
         {
            _pearlSlot = pearl;
            AttachResource(_pearlSlot, _holdPoint, false, Vector3.zero, false, true);
            _pearlSlot.NotifyPickedUp();
            _effects.PlayPickupEffect(transform.position);
            break;
         }
      }

      yield return StartCoroutine(MoveToPosition(_basePosition));

      if (_pearlSlot != null)
      {
         AttachResource(_pearlSlot, null, true, _basePosition, false, false);
         HaveReturned?.Invoke(this, _pearlSlot);
      }

      IsAvailable = true;
      _pearlSlot = null;
   }

   private IEnumerator MoveToPosition(Vector3 targetPosition)
   {
      while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
      {
         transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
         transform.LookAt(targetPosition);
         yield return null;
      }
   }

   private void AttachResource(Pearl pearl, Transform parentPosition, bool isWorldPositionStays, Vector3 localPosition,
                              bool isColliderActive = false, bool isVisible = true)
   {
      if (pearl.TryGetComponent(out Collider collider))
      {
         collider.enabled = isColliderActive;
      }

      pearl.transform.SetParent(parentPosition, isWorldPositionStays);
      pearl.transform.localPosition = localPosition;

      pearl.gameObject.SetActive(isVisible);
   }
}
