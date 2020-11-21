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

            _character.Stats.Q = _tile.Hex.Q;
            _character.Stats.R = _tile.Hex.R;
            _tile.occupyingObject = _character.gameObject;

            if (_character.Stats.lastTile)
            {
                _character.Stats.lastTile.occupyingObject = null;
            }

            _character.Stats.lastTile = _tile;
        }

        protected override IEnumerator AnimationInternal()
        {
            return _character.Animation.MoveRoutine(_tile);
        }
    }
}
