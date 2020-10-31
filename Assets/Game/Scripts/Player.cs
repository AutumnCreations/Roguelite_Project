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
        public WorldTile targetTile = null;

        [Header("Player Movement")]
        [Tooltip("The speed at which the character model will move from their current position to the target position")]
        [SerializeField] private float stepSpeed = 1f;
        [SerializeField] private bool hasControl = true;

        [Tooltip("Temporary value to test enemy characters without AI")]
        [SerializeField] private bool isEnemy = false;

        [Header("Player Stats")]
        [SerializeField] private float health = 10f;

        public float Health { get { return health; } }

        private Camera cam;
        private Vector3 targetPosition;
        private Spell activeSpell;
        //private RaycastHit hit;

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
            if (Input.GetKeyDown(KeyCode.Space)) { hasControl = !hasControl; }
            if (!hasControl) { return; }
            print(currentState);

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
            //Show spell range, target, and VFX

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
        //    private void RaycastCheck()
        //    {
        //        // Check to see if raycast hits another Player (will change to enemy after Proof of Concept) using tag and get reference, then call DisplayCharacterInfo()


        //        //RaycastHit lastHit = hit;
        //        int layerMask = 1 << 8;
        //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(ray, out hit, layerMask))
        //        {
        //            Transform target = hit.transform;
        //            //if (target.CompareTag("Enemy"))
        //            //{
        //                //if (!lastHit.transform || lastHit.transform.CompareTag("Enemy")) { return; }
        //                tooltip.DisplayCharacterInfo(target.GetComponent<Player>());
        //            //}
        //        }
        //            //else if (lastHit.transform && lastHit.transform.CompareTag("Enemy")) { tooltip.HideInfo(); }
        //    }
        #endregion
    }
}
