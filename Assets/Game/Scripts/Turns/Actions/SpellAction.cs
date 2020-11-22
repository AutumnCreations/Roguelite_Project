using System.Collections;
using System.Linq;
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

        private Character _target;

        public SpellAction(Character character, Spell spell, WorldTile targetTile)
        {
            _character = character;
            _spell = spell;
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
                _target.TakeDamage(_spell.Damage);
            }
        }

        protected override IEnumerator AnimationInternal()
        {
            var senderAnimation = _character.Animation.CastSpellRoutine(_spell, _targetTile);
            var targetAnimation = GetTargetAnimation();

            while (true)
            {
                var spell = senderAnimation.MoveNext();
                var damage = targetAnimation?.MoveNext() == true;

                if (spell || damage)
                {
                    yield return null;
                }
                else
                {
                    yield break;
                }
            }
        }

        private IEnumerator GetTargetAnimation()
        {
            if (!_target)
            {
                return null;
            }

            if (_target.Stats.Health > 0)
            {
                return _target?.Animation.TakeDamageRoutine();
            }
            else
            {
                // todo: death animation
                return Enumerable.Empty<int>().GetEnumerator();
            }
        }
    }
}
