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
            if (_tile is null)
            {
                return;
            }

            _character.Q = _tile.Hex.Q;
            _character.R = _tile.Hex.R;
            _tile.occupyingObject = _character.gameObject;

            if (_character.lastTile)
            {
                _character.lastTile.occupyingObject = null;
            }

            _character.lastTile = _tile;
        }

        protected override IEnumerator AnimationInternal()
        {
            return _character.Animation.MoveRoutine(_tile);
        }
    }
}
