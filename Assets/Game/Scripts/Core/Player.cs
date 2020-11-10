using System.Collections.Generic;
using Assets.Game.Scripts.Extensions;
using UnityEngine;

namespace Roguelite.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] public World World;
        [SerializeField] private bool hasControl = true;

        private Camera cam;
        private List<WorldTile> tilesInRange;

        private bool castingSpell;
        private Spell activeSpell;
        private WorldTile targetTile;
        private WorldTile previousTargetTile;

        private Character character;

        protected void Start()
        {
            cam = FindObjectOfType<Camera>();
            tilesInRange = new List<WorldTile>();
            character = transform.GetComponent<Character>();
        }

        private void Update()
        {
            UpdateTargetTile();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                hasControl = !hasControl;
            }

            if (!hasControl)
            {
                return;
            }

            switch (character.CurrentCharacterState)
            {
                case CharacterState.Idle:
                    MovementInput();
                    break;
                case CharacterState.Moving:
                    break;
                case CharacterState.Casting:
                    HandleCasting();
                    break;
            }
        }

        private void MovementInput()
        {
            if (Input.GetMouseButton(0))
            {
                if (targetTile is null)
                {
                    return;
                }

                var distance = targetTile.Hex.DistanceTo(character.Q, character.R);
                if (distance == 1)
                {
                    character.MoveToTile(targetTile);
                }
            }
            else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.UpArrow))
            {
                character.Move(0, 1);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                character.Move(-1, 0);
            }
            else if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.DownArrow))
            {
                character.Move(0, -1);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                character.Move(1, 0);
            }
            else if (Input.GetKey(KeyCode.X))
            {
                character.Move(1, -1);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                character.Move(-1, 1);
            }
        }

        protected void HandleCasting()
        {
            if (!targetTile || castingSpell || !tilesInRange.Contains(targetTile))
            {
                return;
            }

            character.LookAtTarget(targetTile.transform.position);

            if (Input.GetMouseButtonDown(0))
            {
                SetCastingTiles(false);
                StartCoroutine(character.CastSpell(activeSpell, targetTile));
            }
        }

        private void SetCastingTiles(bool isOn)
        {
            if (isOn)
            {
                foreach (var tile in tilesInRange)
                {
                    tile.SetCastingColor();
                }

                return;
            }

            foreach (var tile in tilesInRange)
            {
                tile.SetDefaultColor();
            }
        }

        public void SetActiveSpell(Spell spell)
        {
            if (character.CurrentCharacterState == CharacterState.Casting)
            {
                SetCastingTiles(false);
            }

            activeSpell = spell;
            GetTilesInRange(activeSpell.Range);

            character.CurrentCharacterState = CharacterState.Casting;
            SetCastingTiles(true);
        }

        private void GetTilesInRange(int range)
        {
            tilesInRange.Clear();
            tilesInRange.AddRange(World.GetTilesWithinRange(character.Q, character.R, range));
        }

        private void UpdateTargetTile()
        {
            //if (eventsystem.current.ispointerovergameobject())
            //{ return; }

            // Check to see if over a tile and set the target tile
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(ray);

            foreach (var hit in hits)
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
    }
}
