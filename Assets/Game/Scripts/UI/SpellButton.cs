using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipPopup tooltipPopup;
    [SerializeField] private Spell spell;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = spell.Thumbnail;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPopup.DisplayInfo(spell);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPopup.HideInfo();
    }
}
