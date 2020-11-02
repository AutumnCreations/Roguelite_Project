using System;
using Roguelite.UI;
using UnityEngine;

namespace Roguelite.Core
{
    public class Player : MonoBehaviour
    {
        #region Variables
        [Header("World References")]
        public int X;
        public int Z;
        public World World;

        [Header("Player Movement")]
        [Tooltip("The speed at which the character model will move from their current position to the target position")]
        [SerializeField] private float stepSpeed = 1f;
        [SerializeField] private bool hasControl = true;

        [Tooltip("Temporary value to test enemy characters without AI")]
        [SerializeField] private bool isEnemy = false;

        [Header("Player Stats")]
        [SerializeField] private float health = 10f;

        public float Health { get { return health; } }

        private WorldTile targetTile = null;
        private WorldTile previousTargetTile = null;
        private Camera cam = null;
        private Vector3 targetPosition;
        private Spell activeSpell = null;

        enum State
        {
            Idle,
            Moving,
            Casting,
        }
        State currentState;

        #endregion

        private void Start()
        {
            var tile = World.GetTileAt(X, Z);
            transform.position = tile.transform.position + new Vector3(0, 0.55f);
            targetPosition = transform.position;
            cam = FindObjectOfType<Camera>();

            currentState = State.Idle;
        }

        private void Update()
        {
            if (isEnemy) return;

            TileCheck();

            if (Input.GetKeyDown(KeyCode.Space)) { hasControl = !hasControl; }
            if (!hasControl) { return; }

            //print(currentState);

            switch (currentState)
            {
                case State.Idle:
                    MovementInput();
                    break;
                case State.Moving:
                    UpdateMovement();
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
                float step = stepSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                currentState = State.Moving;
                return;
            }
            currentState = State.Idle;
        }
        private void MovementInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Move(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Move(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(1, 0);
            }
        }
        private void Move(int horizontal, int vertical)
        {
            var tileObject = World.GetTileAt(X + horizontal, Z + vertical);
            if (tileObject is null)
            {
                return;
            }

            targetPosition = tileObject.transform.position + new Vector3(0, 0.55f);

            var tile = tileObject.GetComponent<WorldTile>();
            this.X = tile.X;
            this.Z = tile.Z;

            currentState = State.Moving;
        }
        #endregion

        #region Combat
        private void HandleCasting()
        {
            //To Do: Show spell range, target, and VFX

            //Need to add check for valid target (Enemy/Tile)
            if (Input.GetMouseButtonDown(0) && targetTile)
            {
                CastSpell();
            }
        }

        private void CastSpell()
        {
            Debug.Log("Casting " + activeSpell.name);
            Instantiate(activeSpell.spellEffect, targetTile.transform.position + Vector3.up, targetTile.transform.rotation);
            currentState = State.Idle;
        }

        public void SetActiveSpell(Spell spell)
        {
            activeSpell = spell;
            currentState = State.Casting;
        }
        #endregion

        #region World Interaction
        private void TileCheck()
        {
            // Check to see if over a tile and set the target tile
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    targetTile = hit.transform.GetComponent<WorldTile>();
                    targetTile.SetHighlightedMaterial();

                    if (previousTargetTile && previousTargetTile != targetTile)
                    {
                        previousTargetTile.SetDefaultMaterial();
                    }
                    previousTargetTile = targetTile;
                    return;
                }
            }

            if (hits.Length < 1)
            {
                if (previousTargetTile)
                {
                    previousTargetTile.SetDefaultMaterial();
                }
                targetTile = null;
                previousTargetTile = null;
            }
        }
        #endregion
    }
}
