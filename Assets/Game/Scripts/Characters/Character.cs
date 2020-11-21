using Scripts.UI;
using UnityEngine;

namespace Scripts.Characters
{
    [RequireComponent(typeof(CharacterAnimation))]
    [RequireComponent(typeof(CharacterStats))]
    public class Character : MonoBehaviour
    {
        private TooltipPopup _tooltip;

        public CharacterAnimation Animation { get; private set; }
        public CharacterStats Stats { get; private set; }

        private void Awake()
        {
            Animation = transform.GetComponent<CharacterAnimation>();
            Stats = transform.GetComponent<CharacterStats>();
            _tooltip = FindObjectOfType<TooltipPopup>();
        }

        private void OnMouseOver()
        {
            _tooltip.DisplayCharacterInfo(Stats);
        }

        private void OnMouseExit()
        {
            _tooltip.HideInfo();
        }
    }
}
