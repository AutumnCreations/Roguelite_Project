using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var tile = World.GetTileAt(X, Z);
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
                if (!targetTile) return;

                var currentTile = new Vector2(X, Z);
                var tilePos = new Vector2(targetTile.X, targetTile.Z);

                GetTilesInRange(.75f);

                if (tilePos == currentTile || !tilesInRange.Contains(targetTile)) return;

                MoveToTile(targetTile);
            }

            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.UpArrow))
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
            else if (Input.GetKey(KeyCode.Q))
            {
                Move(-1, 1);
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
            var tileObject = World.GetTileAt(X + horizontal, Z + vertical);
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
            GetTilesInRange(activeSpell.Range * World.XOffset);

            currentState = State.Casting;
            SetCastingTiles(true);
        }

        private void GetTilesInRange(float range)
        {
            tilesInRange.Clear();

            var spellOrginTile = currentState == State.Moving
                ? targetPosition
                : transform.position;


            var collidersInRange = Physics.OverlapSphere(spellOrginTile, range, tileLayerMask);

            foreach (var collider in collidersInRange)
            {
                var tile = collider.GetComponentInParent<WorldTile>();
                tilesInRange.Add(tile);
            }
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
