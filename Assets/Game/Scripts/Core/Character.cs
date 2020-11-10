using System.Collections;
using Scripts.Items;
using UnityEngine;

namespace Scripts.Core
{
    public class Character : MonoBehaviour
    {
        [Header("World References")]
        [SerializeField] public int Q;
        [SerializeField] public int R;
        [SerializeField] public World World;
        [SerializeField] private Vector3 stepOffset = new Vector3(0, .05f);

        [Header("Player Movement")]
        [Tooltip("The speed at which the character model will move from their current position to the target position")]
        [SerializeField] private float stepSpeed = 1f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private Animator animator;

        public CharacterState CurrentCharacterState;
        private Vector3 targetPosition;

        protected void Start()
        {
            var random = new System.Random();
            var tile = World.GetTileAt(Q, R);
            while (tile is null)
            {
                tile = World.GetTileAt(random.Next(11) - 5, random.Next(11) - 5);
                R = tile.Hex.R;
                Q = tile.Hex.Q;
            }

            transform.position = tile.transform.position + stepOffset;
            targetPosition = transform.position;

            CurrentCharacterState = CharacterState.Idle;
        }

        protected void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (transform.position != targetPosition)
            {
                LookAtTarget(targetPosition);
                var step = stepSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }
            else
            {
                animator.SetBool("isMoving", false);
                if (CurrentCharacterState == CharacterState.Moving)
                {
                    CurrentCharacterState = CharacterState.Idle;
                }
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

        public void Move(int horizontal, int vertical)
        {
            var target = World.GetTileAt(Q + horizontal, R + vertical);
            MoveToTile(target);
        }

        public void MoveToTile(WorldTile target)
        {
            if (target is null)
            {
                return;
            }

            targetPosition = target.transform.position + stepOffset;
            Q = target.Hex.Q;
            R = target.Hex.R;
            animator.SetBool("isMoving", true);
            CurrentCharacterState = CharacterState.Moving;
        }

        public IEnumerator CastSpell(Spell activeSpell, WorldTile targetTile)
        {
            Instantiate(activeSpell.spellEffect, targetTile.transform.position + Vector3.up, targetTile.transform.rotation);
            animator.SetTrigger("castSpell");

            yield return new WaitForSeconds(2.3f);
            CurrentCharacterState = CharacterState.Idle;
        }
    }
}
