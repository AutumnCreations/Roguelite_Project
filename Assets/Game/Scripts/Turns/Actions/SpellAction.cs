using Scripts.Characters;
using Scripts.Items;
using Scripts.Worlds;

namespace Scripts.Turns.Actions
{
    public class SpellAction : TurnAction
    {
        private readonly Character _character;
        private readonly Spell _spell;
        private readonly WorldTile _targetTile;

        public SpellAction(Character character, Spell spell, WorldTile targetTile)
        {
            _character = character;
            _spell = spell;
            _targetTile = targetTile;
        }

        public override void CastSpell()
        {
            _character.CastSpell(_spell, _targetTile);
        }
    }
}
