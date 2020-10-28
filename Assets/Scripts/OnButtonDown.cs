using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnButtonDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string key;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.GetComponent<MobileControls>().ButtonPressHandler(key);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.parent.GetComponent<MobileControls>().ButtonUpHandler(key);
    }
}
