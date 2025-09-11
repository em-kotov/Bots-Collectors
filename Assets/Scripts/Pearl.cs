using UnityEngine;

public class Pearl : MonoBehaviour
{
   public System.Action<Pearl> PickedUp;

   public void NotifyPickedUp()
   {
      PickedUp?.Invoke(this);
   }
}
