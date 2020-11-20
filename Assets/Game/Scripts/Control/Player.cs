using System.Collections.Generic;
using Scripts.Characters;
using Scripts.Extensions;
using Scripts.Items;
using Scripts.Turns.Actions;
using Scripts.Worlds;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Control
{
    public class Player : AbstractControl
    {
        [SerializeField] public World World;

        [Tooltip("Related to camera controls")]
        [SerializeField] private bool hasControl = true;

        private Character _character;

        private UnityEngine.Camera cam;
        private List<WorldTile> spellRange;

        private Spell activeSpell;
        private WorldTile targetTile;
        private WorldTile previousTargetTile;

        private TurnAction _nextAction;

        private void Start()
        {
            _character = transform.GetComponent<Character>();
            cam = FindObjectOfType<UnityEngine.Camera>();
            spellRange = new List<WorldTile>();
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

            PlayerInput();
        }

        public override TurnAction NextAction()
        {
            var nextAction = _nextAction;
            _nextAction = null;
            return nextAction;
        }

        public void SetActiveSpell(Spell spell)
        {
            if (!(activeSpell is null))
            {
                var previous = activeSpell;
                ShowSpellRange(false);
                spellRange.Clear();
                activeSpell = null;

                if (previous == spell)
                {
                    return;
                }
            }

            activeSpell = spell;
            GetTilesInRange(activeSpell.Range);
            ShowSpellRange(true);
        }

        private void GetTilesInRange(int range)
        {
            spellRange.Clear();
            spellRange.AddRange(World.GetTilesWithinRange(_character.Q, _character.R, range));
        }

        private void UpdateTargetTile()
        {
            if (MouseInputUIBlocker.BlockedByUI)
            {
                if (previousTargetTile)
                {
                    previousTargetTile.ResetTileColor();
                }

                targetTile = null;
                previousTargetTile = null;
                return;
            }

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

        private void PlayerInput()
        {
            TurnAction action = default;

            if (Input.GetMouseButtonDown(0))
            {
                if (targetTile)
                {
                    if (activeSpell is null)
                    {
                        var distance = targetTile.Hex.DistanceTo(_character.Q, _character.R);
                        action = distance == 1
                            ? new MoveAction(_character, targetTile)
                            : null;
                    }
                    else if (spellRange.Contains(targetTile))
                    {
                        action = CastSpell();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                action = MoveAction(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                action = MoveAction(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                action = MoveAction(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                action = MoveAction(1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                action = MoveAction(1, -1);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                action = MoveAction(-1, 1);
            }
            else
            {
                return;
            }

            _nextAction = action;
        }

        private MoveAction MoveAction(int q, int r)
        {
            var tile = World.GetTileAt(_character.Q + q, _character.R + r);
            return tile is null
                ? null
                : new MoveAction(_character, tile);
        }

        private SpellAction CastSpell()
        {
            var action = new SpellAction(_character, activeSpell, targetTile);

            ShowSpellRange(false);
            spellRange.Clear();
            activeSpell = null;

            return action;
        }

        private void ShowSpellRange(bool isOn)
        {
            if (isOn)
            {
                foreach (var tile in spellRange)
                {
                    tile.SetCastingColor();
                }
            }
            else
            {
                foreach (var tile in spellRange)
                {
                    tile.SetDefaultColor();
                }
            }
        }
    }
}
