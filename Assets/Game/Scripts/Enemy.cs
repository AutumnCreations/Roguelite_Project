using UnityEngine;
using Roguelite.UI;

namespace Roguelite.Combat
{
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy Stats")]
        [SerializeField] private float health = 10f;

        public float Health { get { return health; } }

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
