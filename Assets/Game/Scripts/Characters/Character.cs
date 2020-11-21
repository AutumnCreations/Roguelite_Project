using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Items;
using Scripts.UI;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Characters
{
    [RequireComponent(typeof(CharacterAnimation))]
    public class Character : MonoBehaviour
    {
        [Header("Location")]
        public int Q;
        public int R;
        public WorldTile lastTile = null;

        [Header("Combat")]
        [SerializeField] private float health = 10f;
        [SerializeField] public List<Spell> spells;

        public float Health
        {
            get => health;
            set => health = value;
        }

        public CharacterAnimation Animation;
        private TooltipPopup tooltip;

        private void Start()
        {
            Animation = transform.GetComponent<CharacterAnimation>();
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
