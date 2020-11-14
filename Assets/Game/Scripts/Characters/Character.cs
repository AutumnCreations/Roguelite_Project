using System.Collections;
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

        [Header("Combat")]
        [SerializeField] private float health = 10f;

        public float Health => health;

        private CharacterAnimation Animation;
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

        public IEnumerator CastSpellRoutine(Spell spell, WorldTile targetTile)
        {
            return Animation.CastSpellRoutine(spell, targetTile);
        }

        public IEnumerator MoveRoutine(WorldTile tile)
        {
            if (tile is null)
            {
                yield break;
            }

            yield return Animation.MoveRoutine(tile);
            Q = tile.Hex.Q;
            R = tile.Hex.R;
        }
    }
}
