using System.Collections;
using Scripts.Characters;
using Scripts.Items;
using Scripts.Worlds;

namespace Scripts.Turns.Actions
{
    public class SpellAction : TurnAction
    {
        private readonly Character _character;
        private readonly Spell _activeSpell;
        private readonly WorldTile _targetTile;

        public SpellAction(Character character, Spell activeSpell, WorldTile targetTile)
        {
            _character = character;
            _activeSpell = activeSpell;
            _targetTile = targetTile;
        }

        protected override IEnumerator ActInternal()
        {
            return _character.CastSpellRoutine(_activeSpell, _targetTile);
        }
    }
}
