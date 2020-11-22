using Scripts.Turns.Actions;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Control
{
    public class Enemy : AbstractControl
    {
        private static readonly System.Random Random = new System.Random();

        [SerializeField] public World World;

        public override TurnAction NextAction()
        {
            var q = Random.Next(3) - 1;
            var r = Random.Next(3) - 1;
            if (q == r)
            {
                return new NoAction();
            }

            return MoveAction(q, r);
        }

        private MoveAction MoveAction(int q, int r)
        {
            var tile = World.GetTileAt(Character.Stats.Q + q, Character.Stats.R + r);
            return new MoveAction(Character, tile);
        }
    }
}
