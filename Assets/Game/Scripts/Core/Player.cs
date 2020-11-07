using System;
using System.Collections.Generic;
using Roguelite.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Roguelite.Core
{
    public class Player : MonoBehaviour
    {
        #region Variables
        [Header("World References")]
        public int X;
        public int Z;
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
        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, spellRangeVisualizer);
        }
        #endregion

        private void Start()
        {
            cam = FindObjectOfType<Camera>();
            WorldTile tile = World.GetTileAt(X, Z);
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
                float step = stepSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                //currentState = State.Moving;
            }
            else
            {
                animator.SetBool("isMoving", false);

                if (currentState == State.Moving) currentState = State.Idle;
            }
        }

        private void LookAtTarget(Vector3 lookTarget)
        {
            Vector3 dir = lookTarget - transform.position;
            dir.y = 0; // keep the direction strictly horizontal
            Quaternion rot = Quaternion.LookRotation(dir);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        private void MovementInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!targetTile) return;

                Vector2 currentTile = new Vector2(X, Z);
                Vector2 tilePos = new Vector2(targetTile.X, targetTile.Z);

                print("Current Tile: " + currentTile);
                print("Next Tile: " + tilePos);

                GetTilesInRange(.75f);

                if (tilePos == currentTile || !tilesInRange.Contains(targetTile)) return;

                MoveToTile(targetTile);

            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(1, 0);
            }
        }

        private void MoveToTile(WorldTile target)
        {
            targetPosition = target.transform.position + stepOffset;
            X = target.X;
            Z = target.Z;
            animator.SetBool("isMoving", true);
            currentState = State.Moving;
        }

        private void Move(int horizontal, int vertical)
        {
            WorldTile tileObject = World.GetTileAt(X + horizontal, Z + vertical);
            if (tileObject is null)
            {
                return;
            }

            targetPosition = tileObject.transform.position + stepOffset;

            X = tileObject.X;
            Z = tileObject.Z;


            animator.SetBool("isMoving", true);
            currentState = State.Moving;
        }
        #endregion

        #region Combat
        private void HandleCasting()
        {
            if (!targetTile) return;

            if (tilesInRange.Contains(targetTile))
            {
                LookAtTarget(targetTile.transform.position);
            }

            if (Input.GetMouseButtonDown(0))
            {
                CastSpell();
            }
        }

        private void CastSpell()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            { return; }
            if (!tilesInRange.Contains(targetTile)) { return; }

            //Debug.Log("Casting " + activeSpell.name);
            Instantiate(activeSpell.spellEffect, targetTile.transform.position + Vector3.up, targetTile.transform.rotation);

            animator.SetTrigger("castSpell");
            //animator.SetBool("isCasting", false);
            currentState = State.Idle;
            SetCastingTiles(false);
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

            GetTilesInRange(activeSpell.Range * World.XOffset);

            //animator.SetBool("isCasting", true);
            currentState = State.Casting;
            SetCastingTiles(true);
        }

        private void GetTilesInRange(float range)
        {
            tilesInRange.Clear();

            Vector3 spellOrginTile;

            if (currentState == State.Moving)
            { spellOrginTile = targetPosition; }
            else
            { spellOrginTile = transform.position; }

            Collider[] collidersInRange = Physics.OverlapSphere(spellOrginTile, range, tileLayerMask);

            foreach (Collider collider in collidersInRange)
            {
                WorldTile tile = collider.GetComponentInParent<WorldTile>();
                tilesInRange.Add(tile);
            }
        }
        #endregion

        #region World Interaction
        private void TileCheck()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            { return; }

            // Check to see if over a tile and set the target tile
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
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
