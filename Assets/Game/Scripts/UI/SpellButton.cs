using Scripts.Control;
using Scripts.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class SpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Spell spell;
        public Player player;
        private TooltipPopup tooltipPopup;
        private Image image;


        private void Start()
        {
            image = GetComponent<Image>();
            image.sprite = spell.Thumbnail;
            tooltipPopup = FindObjectOfType<TooltipPopup>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipPopup.DisplayItemInfo(spell);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipPopup.HideInfo();
        }

        public void SetActiveSpell()
        {
            player.SetActiveSpell(spell);
        }
    }
}
