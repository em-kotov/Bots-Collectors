using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FortClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Action OnSelected;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        OnSelected?.Invoke();
    }
}