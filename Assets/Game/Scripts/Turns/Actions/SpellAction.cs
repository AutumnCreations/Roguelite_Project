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

        private Character _target;

        public SpellAction(Character character, Spell activeSpell, WorldTile targetTile)
        {
            _character = character;
            _activeSpell = activeSpell;
            _targetTile = targetTile;
        }

        public override void CastSpell()
        {
            if (!_targetTile.occupyingObject)
            {
                return;
            }

            _target = _targetTile.occupyingObject.GetComponent<Character>();
            if (_target)
            {
                _target.Health -= _activeSpell.Damage;
            }
        }

        protected override IEnumerator AnimationInternal()
        {
            var r1 = _target?.Animation.TakeDamageRoutine();
            var r2 = _character.Animation.CastSpellRoutine(_activeSpell, _targetTile);

            while (true)
            {
                var damage = r1?.MoveNext() == true;
                var spell = r2.MoveNext();

                if (damage || spell)
                {
                    yield return null;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
