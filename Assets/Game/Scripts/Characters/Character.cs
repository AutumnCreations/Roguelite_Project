using Scripts.Items;
using Scripts.UI;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Characters
{
    [RequireComponent(typeof(CharacterAnimation))]
    [RequireComponent(typeof(CharacterStats))]
    public class Character : MonoBehaviour
    {
        [SerializeField] public WorldTile CurrentTile;

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

        public void MoveTo(WorldTile tile)
        {
            if (tile is null)
            {
                return;
            }

            Stats.Q = tile.Hex.Q;
            Stats.R = tile.Hex.R;
            tile.occupyingObject = gameObject;

            if (CurrentTile)
            {
                CurrentTile.occupyingObject = null;
            }

            CurrentTile = tile;

            Animation.Move(tile);
        }

        public void CastSpell(Spell spell, WorldTile targetTile)
        {
            Animation.CastSpell(spell, targetTile);

            if (!targetTile.occupyingObject)
            {
                return;
            }

            var target = targetTile.occupyingObject.GetComponent<Character>();
            if (target)
            {
                target.TakeDamage(spell.Damage);
            }
        }

        public void TakeDamage(int amount)
        {
            Stats.Health -= amount;

            if (Stats.Health > 0)
            {
                Animation.TakeDamage();
            }
            else
            {
                // todo: death animation
            }
        }
    }
}
