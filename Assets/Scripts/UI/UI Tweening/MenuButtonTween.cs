using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text Color")]
    public Color normalText = Color.white;
    public Color highlightedText = Color.black;

    [Header("Button Color")]
    public Color normalButton = Color.clear;
    public Color highlightedButton = Color.white;

    Image myImage;
    Text buttonText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //LeanTween.textColor(buttonText.rectTransform, Color.black, 0.15f).setIgnoreTimeScale(true);
        //LeanTween.color(myImage.rectTransform, Color.white, 0.15f).setIgnoreTimeScale(true);

        LeanTween.textColor(buttonText.rectTransform, highlightedText, 0.15f).setIgnoreTimeScale(true);
        LeanTween.color(myImage.rectTransform, highlightedButton, 0.15f).setIgnoreTimeScale(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //LeanTween.textColor(buttonText.rectTransform, Color.white, 0.15f).setIgnoreTimeScale(true);
        //LeanTween.color(myImage.rectTransform, Color.clear, 0.15f).setIgnoreTimeScale(true);

        LeanTween.textColor(buttonText.rectTransform, normalText, 0.15f).setIgnoreTimeScale(true);
        LeanTween.color(myImage.rectTransform, normalButton, 0.15f).setIgnoreTimeScale(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<Text>();
    }

}
