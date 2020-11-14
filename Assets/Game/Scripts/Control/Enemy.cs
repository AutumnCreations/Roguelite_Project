using Scripts.Characters;
using Scripts.Turns.Actions;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Control
{
    [RequireComponent(typeof(Character))]
    public class Enemy : AbstractControl
    {
        private static readonly System.Random Random = new System.Random();

        [SerializeField] public World World;

        private Character _character;

        private void Start()
        {
            _character = transform.GetComponent<Character>();
        }

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
            var tile = World.GetTileAt(_character.Q + q, _character.R + r);
            return new MoveAction(_character, tile);
        }
    }
}
