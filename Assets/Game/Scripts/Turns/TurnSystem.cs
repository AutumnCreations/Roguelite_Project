using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Control;
using Scripts.Turns.Actions;
using UnityEngine;

namespace Scripts.Turns
{
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private List<Enemy> _enemies;

        private bool _isPlanningPhase;
        private List<TurnAction> _currentTurn;

        private void Awake()
        {
            _isPlanningPhase = true;
            _currentTurn = new List<TurnAction>();
        }

        private void FixedUpdate()
        {
            if (_isPlanningPhase)
            {
                OnAction();
            }
            else
            {
                Wait();
            }
        }

        private void OnAction()
        {
            var playerAction = _player.NextAction();
            if (playerAction == null)
            {
                return;
            }

            _isPlanningPhase = false;

            _currentTurn.Add(playerAction);
            _currentTurn.AddRange(_enemies.Select(e => e.NextAction()));

            foreach (var action in _currentTurn)
            {
                action.Move();
            }

            foreach (var action in _currentTurn)
            {
                action.CastSpell();
            }

            foreach (var action in _currentTurn)
            {
                StartCoroutine(action.Animation());
            }
        }

        private void Wait()
        {
            if (_currentTurn.Any(r => !r.Completed))
            {
                return;
            }

            _currentTurn.Clear();
            _isPlanningPhase = true;
        }
    }
}
