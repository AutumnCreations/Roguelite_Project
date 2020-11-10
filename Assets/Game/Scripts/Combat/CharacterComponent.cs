using UnityEngine;
using Roguelite.UI;

namespace Roguelite.Combat
{
    public class CharacterComponent : MonoBehaviour
    {
        [Header("Character Stats")]
        [SerializeField] private float health = 10f;

        public float Health => health;

        private TooltipPopup tooltip;

        private void Start()
        {
            tooltip = FindObjectOfType<TooltipPopup>();
        }

        private void OnMouseOver()
        {
            tooltip.DisplayCharacterInfo(this);
        }

        private void OnMouseExit()
        {
            tooltip.HideInfo();
        }
    }
}
