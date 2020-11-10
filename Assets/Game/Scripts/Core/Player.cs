﻿using System.Collections;
using System.Collections.Generic;
using Assets.Game.Scripts.Extensions;
using UnityEngine;

namespace Roguelite.Core
{
    public class Player : MonoBehaviour
    {
        #region Variables
        [Header("World References")]
        public int Q;
        public int R;
        public World World;
        [SerializeField] private Vector3 stepOffset = new Vector3(0, .05f);

        [Header("Player Movement")]
        [Tooltip("The speed at which the character model will move from their current position to the target position")]
        [SerializeField] private float stepSpeed = 1f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private bool hasControl = true;
        [SerializeField] private Animator animator = null;

        [Tooltip("Temporary value to test enemy characters without AI")]
        [SerializeField] private bool isEnemy = false;

        [Header("Player Stats")]
        [SerializeField] private float health = 10f;

        [Header("Debugging")]
        [SerializeField] private float spellRangeVisualizer = 5f;

        public float Health { get { return health; } }

        private bool spellCasted = false;

        private WorldTile targetTile = null;
        private List<WorldTile> tilesInRange = null;
        private WorldTile previousTargetTile = null;
        private Camera cam = null;
        private Vector3 targetPosition;
        private Spell activeSpell = null;
        private LayerMask tileLayerMask;


        enum State
        {
            Idle,
            Moving,
            Casting,
        }
        State currentState;

        #endregion

        #region Debugging
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, spellRangeVisualizer);
        }
        #endregion

        private void Start()
        {
            cam = FindObjectOfType<Camera>();
            var tile = World.GetTileAt(Q, R);
            transform.position = tile.transform.position + stepOffset;
            targetPosition = transform.position;

            tileLayerMask = LayerMask.GetMask("Tile");
            tilesInRange = new List<WorldTile>();
            spellRangeVisualizer *= World.XOffset;

            currentState = State.Idle;
        }

        private void Update()
        {
            if (isEnemy) return;

            TileCheck();
            UpdateMovement();

            if (Input.GetKeyDown(KeyCode.Space)) { hasControl = !hasControl; }
            if (!hasControl) { return; }

            switch (currentState)
            {
                case State.Idle:
                    MovementInput();
                    break;
                case State.Moving:
                    break;
                case State.Casting:
                    HandleCasting();
                    break;
            }
        }

        #region Movement
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
                if (currentState == State.Moving) currentState = State.Idle;
            }
        }

        private void LookAtTarget(Vector3 lookTarget)
        {
            var direction = lookTarget - transform.position;
            //Keep the direction strictly horizontal
            direction.y = 0;
            var targetRotation = Quaternion.LookRotation(direction);

            //Slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void MovementInput()
        {
            if (Input.GetMouseButton(0))
            {
                if (targetTile is null)
                {
                    return;
                }

                var distance = targetTile.Hex.DistanceTo(Q, R);
                if (distance == 1)
                {
                    MoveToTile(targetTile);
                }
            }
            else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.UpArrow))
            {
                Move(0, 1);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Move(-1, 0);
            }
            else if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.DownArrow))
            {
                Move(0, -1);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Move(1, 0);
            }
            else if (Input.GetKey(KeyCode.X))
            {
                Move(1, -1);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                Move(-1, 1);
            }
        }

        private void Move(int horizontal, int vertical)
        {
            var target = World.GetTileAt(Q + horizontal, R + vertical);
            MoveToTile(target);
        }

        private void MoveToTile(WorldTile target)
        {
            if (target is null)
            {
                return;
            }

            targetPosition = target.transform.position + stepOffset;
            Q = target.Hex.Q;
            R = target.Hex.R;
            animator.SetBool("isMoving", true);
            currentState = State.Moving;
        }

        #endregion

        #region Combat
        private void HandleCasting()
        {
            if (!targetTile || spellCasted) return;

            if (tilesInRange.Contains(targetTile))
            {
                LookAtTarget(targetTile.transform.position);
            }

            if (Input.GetMouseButtonDown(0))
            {
                //Checks if cursor is over UI element
                if (!tilesInRange.Contains(targetTile))
                { return; }

                StartCoroutine(CastSpell());
            }
        }

        private IEnumerator CastSpell()
        {
            Instantiate(activeSpell.spellEffect, targetTile.transform.position + Vector3.up, targetTile.transform.rotation);
            animator.SetTrigger("castSpell");
            SetCastingTiles(false);
            spellCasted = true;

            yield return new WaitForSeconds(2.3f);
            currentState = State.Idle;
            spellCasted = false;
        }

        private void SetCastingTiles(bool isOn)
        {
            if (isOn)
            {
                foreach (WorldTile tile in tilesInRange)
                {
                    tile.SetCastingColor();
                }
                return;
            }

            foreach (WorldTile tile in tilesInRange)
            {
                tile.SetDefaultColor();
            }
        }

        public void SetActiveSpell(Spell spell)
        {
            if (currentState == State.Casting) { SetCastingTiles(false); }

            activeSpell = spell;
            GetTilesInRange(activeSpell.Range);

            currentState = State.Casting;
            SetCastingTiles(true);
        }

        private void GetTilesInRange(int range)
        {
            tilesInRange.Clear();
            tilesInRange.AddRange(World.GetTilesWithinRange(Q, R, range));
        }
        #endregion

        #region World Interaction
        private void TileCheck()
        {
            //if (eventsystem.current.ispointerovergameobject())
            //{ return; }

            // Check to see if over a tile and set the target tile
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    targetTile = hit.transform.GetComponentInParent<WorldTile>();
                    targetTile.SetHoverColor();

                    if (previousTargetTile && previousTargetTile != targetTile)
                    {
                        previousTargetTile.ResetTileColor();
                    }
                    previousTargetTile = targetTile;
                    return;
                }
            }

            if (hits.Length < 1)
            {
                if (previousTargetTile)
                {
                    previousTargetTile.ResetTileColor();
                }
                targetTile = null;
                previousTargetTile = null;
            }
        }
        #endregion
    }
}
