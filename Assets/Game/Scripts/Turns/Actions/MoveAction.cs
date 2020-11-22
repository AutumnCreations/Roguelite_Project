using System.Collections;
using Scripts.Characters;
using Scripts.Worlds;

namespace Scripts.Turns.Actions
{
    public class MoveAction : TurnAction
    {
        private readonly Character _character;
        private readonly WorldTile _tile;

        public MoveAction(Character character, WorldTile tile)
        {
            _character = character;
            _tile = tile;
        }

        public override void Move()
        {
            _character.MoveTo(_tile);
        }

        protected override IEnumerator AnimationInternal()
        {
            return _character.Animation.MoveRoutine(_tile);
        }
    }
}
