using UnityEngine;

public class Pearl : MonoBehaviour
{
   [SerializeField] private Collider _collider;

   public System.Action<Pearl> PickedUp;
   public System.Action<Pearl> ReadyToReturn;

   public void Initialize()
   {
      _collider.enabled = true;
   }

   public void NotifyPickedUp()
   {
      PickedUp?.Invoke(this);
   }

   public void NotifyReadyToReturn()
   {
      ReadyToReturn?.Invoke(this);
   }
}
