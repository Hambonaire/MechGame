using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FadeItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        HangarManager._instance.FadeOutItemUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HangarManager._instance.FadeInItemUI();
    }
}
