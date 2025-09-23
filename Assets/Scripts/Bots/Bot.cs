using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
   [SerializeField] private Transform _holdPoint;
   [SerializeField] private BotEffects _effects;
   [SerializeField] private BotMover _mover;

   private Pearl _pearlSlot;
   private Vector3 _basePosition;

   public Action<Bot, Pearl> HaveReturned;
   public Action<Bot> BecomeAvailable;

   public void Initialize()
   {
      transform.position = _basePosition;
      _effects.OverrideMaterialColor((Renderer[])GetRenderersInChildren());
      NotifyAvailable();
   }

   public void SetBasePosition(Vector3 position)
   {
      _basePosition = position;
   }

   public void Collect(Pearl targetPearl)
   {
      StartCoroutine(CollectRoutine(targetPearl));
   }

   private IEnumerator CollectRoutine(Pearl targetPearl)
   {
      _mover.MoveToTarget(transform, targetPearl.transform.position);

      yield return new WaitUntil(() => _mover.IsEnoughClose(transform.position, targetPearl.transform.position) == true);

      _pearlSlot = targetPearl;
      AttachResource(_pearlSlot, _holdPoint, false, Vector3.zero, false, true);
      _pearlSlot.NotifyPickedUp();
      _effects.PlayPickupEffect(transform.position);

      _mover.MoveToTarget(transform, _basePosition);

      yield return new WaitUntil(() => _mover.IsEnoughClose(transform.position, _basePosition) == true);

      if (_pearlSlot != null)
      {
         AttachResource(_pearlSlot, null, true, _basePosition, false, false);
         HaveReturned?.Invoke(this, _pearlSlot);
      }

      NotifyAvailable();
      _pearlSlot = null;
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

   private void NotifyAvailable()
   {
      BecomeAvailable?.Invoke(this);
   }

#if UNITY_EDITOR
   [ContextMenu("Refresh Renderers In Children")]
   private Array GetRenderersInChildren()
   {
      HashSet<Renderer> renderers = new HashSet<Renderer>();
      int childCount = transform.childCount;

      for (int i = 0; i < childCount; i++)
      {
         if (transform.GetChild(i).gameObject.TryGetComponent(out Renderer renderer))
         {
            renderers.Add(renderer);
         }
      }

      return renderers.ToArray();
   }
#endif
}
