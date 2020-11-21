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
            if (targetTile.occupyingObject)
            {
                DealDamage(spell, targetTile);
            }
            return Animation.CastSpellRoutine(spell, targetTile);
        }

        private void DealDamage(Spell spell, WorldTile targetTile)
        {
            var target = targetTile.occupyingObject.GetComponent<Character>();
            if (target)
            {
                StartCoroutine(target.TakeDamageRoutine(spell.Damage));
            }
        }

        public IEnumerator TakeDamageRoutine(int damage)
        {
            health -= damage;
            print(gameObject.name + " " + health);
            if (health > 0)
            {
                return Animation.TakeDamageRoutine();
            }
            else
            {
                return Enumerable.Empty<int>().GetEnumerator();
            }
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
            tile.occupyingObject = gameObject;

            if (lastTile)
            {
                lastTile.occupyingObject = null;
            }

            lastTile = tile;
        }
    }
}
