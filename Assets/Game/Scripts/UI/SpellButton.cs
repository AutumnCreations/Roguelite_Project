using Roguelite.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Roguelite.UI
{
    public class SpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Spell spell;

        private Player player;
        private TooltipPopup tooltipPopup;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
            image.sprite = spell.Thumbnail;
            tooltipPopup = FindObjectOfType<TooltipPopup>();

            //Using tag reference to find player to avoid conflicts with enemy test dummies
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
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
