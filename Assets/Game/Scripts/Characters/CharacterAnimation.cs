using System.Collections;
using Scripts.Items;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Characters
{
    public class CharacterAnimation : MonoBehaviour
    {
        [Header("Character Movement")]
        [Tooltip("The speed at which the character model will move from their current position to the target position")]
        [SerializeField] private float stepSpeed = 1f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private Vector3 stepOffset = new Vector3(0, .05f);
        [SerializeField] private Animator animator;

        public CharacterAnimationState CurrentCharacterAnimationState;
        private Vector3 _targetPosition;

        protected void Start()
        {
            _targetPosition = transform.position;
            CurrentCharacterAnimationState = CharacterAnimationState.Idle;
        }

        public IEnumerator MoveRoutine(WorldTile target)
        {
            if (target is null)
            {
                yield break;
            }

            _targetPosition = target.transform.position + stepOffset;
            animator.SetBool(AnimatorConstants.IsMoving, true);
            CurrentCharacterAnimationState = CharacterAnimationState.Moving;

            while (transform.position != _targetPosition)
            {
                LookAtTarget(_targetPosition);
                var step = stepSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
                yield return null;
            }

            animator.SetBool(AnimatorConstants.IsMoving, false);
            if (CurrentCharacterAnimationState == CharacterAnimationState.Moving)
            {
                CurrentCharacterAnimationState = CharacterAnimationState.Idle;
            }
        }

        public IEnumerator CastSpellRoutine(Spell activeSpell, WorldTile targetTile)
        {
            CurrentCharacterAnimationState = CharacterAnimationState.Casting;

            var newSpell = Instantiate(activeSpell.spellEffect, targetTile.transform.position + Vector3.up, targetTile.transform.rotation);
            Destroy(newSpell, newSpell.GetComponent<ParticleSystem>().main.duration);

            animator.SetTrigger(AnimatorConstants.CastSpell);

            var endOfAnimation = Time.time + 2.3f;
            while (Time.time < endOfAnimation)
            {
                LookAtTarget(targetTile.transform.position);
                yield return null;
            }

            CurrentCharacterAnimationState = CharacterAnimationState.Idle;
        }

        public IEnumerator TakeDamageRoutine()
        {
            animator.SetTrigger(AnimatorConstants.TakeDamage);

            var endOfAnimation = Time.time + 2.3f;
            while (Time.time < endOfAnimation)
            {
                yield return null;
            }
        }

        public void LookAtTarget(Vector3 lookTarget)
        {
            var direction = lookTarget - transform.position;
            //Keep the direction strictly horizontal
            direction.y = 0;
            var targetRotation = Quaternion.LookRotation(direction);

            //Slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
