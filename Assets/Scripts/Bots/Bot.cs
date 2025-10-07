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
   private Vector3 _pearlStoragePosition;
   private Vector3 _parkingPosition;
   private bool _isAvailable = true;

   public Action<Bot, Pearl> HaveReturned;
   public Action<Bot> BecomeAvailable;
   public Action<Bot> ReachedFlag;

   public void Initialize()
   {
      _mover.SetBotTransform(transform);
      _effects.OverrideMaterialColor((Renderer[])GetRenderersInChildren());
   }

   public void AssignToFort(Vector3 storagePosition)
   {
      _pearlStoragePosition = storagePosition;
      NotifyAvailable();
   }

   public void SetParkingPosition(Vector3 position)
   {
      _parkingPosition = position;

      if (_isAvailable)
      {
         _mover.GoToPosition(_parkingPosition);
      }
   }

   public void Collect(Pearl targetPearl)
   {
      StartCoroutine(CollectRoutine(targetPearl));
   }

   public void GoToFlag(Flag flag, Fort original)
   {
      StartCoroutine(BuildFort(flag, original));
   }

   private IEnumerator CollectRoutine(Pearl targetPearl)
   {
      _isAvailable = false;
      _mover.GoToTransform(targetPearl.transform);

      yield return new WaitUntil(() => _mover.HasReachedTarget);

      _pearlSlot = targetPearl;
      AttachResource(_pearlSlot, _holdPoint, false, Vector3.zero, false, true);
      _pearlSlot.NotifyPickedUp();
      _effects.PlayPickupEffect(transform.position);

      _mover.GoToPosition(_parkingPosition);

      yield return new WaitUntil(() => _mover.HasReachedTarget);

      if (_pearlSlot != null)
      {
         AttachResource(_pearlSlot, null, true, _pearlStoragePosition, false, false);
         HaveReturned?.Invoke(this, _pearlSlot);
      }

      _pearlSlot = null;
      _isAvailable = true;
      NotifyAvailable();
   }

   private IEnumerator BuildFort(Flag flag, Fort original)
   {
      _isAvailable = false;
      _mover.GoToTransform(flag.transform);

      yield return new WaitUntil(() => _mover.HasReachedTarget);

      flag.CreateFort(original, this);
      ReachedFlag?.Invoke(this);
      _isAvailable = true;
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
      if (_isAvailable)
      {
         BecomeAvailable?.Invoke(this);
      }
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
