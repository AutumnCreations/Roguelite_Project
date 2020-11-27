using System.Linq;
using Scripts.Extensions;
using Scripts.Items;
using Scripts.Turns.Actions;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Control
{
    public class Enemy : AbstractControl
    {
        private static readonly System.Random Random = new System.Random();

        [HideInInspector]
        public World World;

        private void Start()
        {
            World = FindObjectOfType<World>();
        }

        public override TurnAction NextAction()
        {
            if (Character.Stats.Health <= 0)
            {
                return new NoAction();
            }

            var q = Random.Next(3) - 1;
            var r = Random.Next(3) - 1;
            if (q != r)
            {
                return MoveAction(q, r);
            }

            var spell = Character.Stats.Spells.FirstOrDefault();
            if (spell is null)
            {
                return new NoAction();
            }

            var target = World.GetCharactersWithinRange(Character.Stats.Q, Character.Stats.R, spell.Range)
                .Where(c => c != Character)
                .Select(c => c.CurrentTile)
                .Shuffle()
                .FirstOrDefault();
            if (target)
            {
                return SpellAction(spell, target);
            }

            var targetTile = World.GetTilesWithinRange(Character.Stats.Q, Character.Stats.R, spell.Range)
                .Shuffle()
                .FirstOrDefault();
            if (targetTile)
            {
                return SpellAction(spell, targetTile);
            }

            return new NoAction();
        }

        private MoveAction MoveAction(int q, int r)
        {
            var tile = World.GetTileAt(Character.Stats.Q + q, Character.Stats.R + r);
            return new MoveAction(Character, tile);
        }

        private SpellAction SpellAction(Spell spell, WorldTile target)
        {
            return new SpellAction(Character, spell, target);
        }
    }
}
