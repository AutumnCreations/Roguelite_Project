using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Characters;
using Scripts.Control;
using Scripts.Turns.Actions;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Turns
{
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private EnemyContainer _enemies;

        private bool _isPlanningPhase;
        private List<TurnAction> _currentTurn;
        private IEnumerable<Character> _characters;

        private void Awake()
        {
            _isPlanningPhase = true;
            _currentTurn = new List<TurnAction>();
        }

        private void Start()
        {
            _characters = new[] { _player.Character }.Concat(_enemies.Characters.Select(e => e.Character));
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
            _currentTurn.AddRange(_enemies.Characters.Select(e => e.NextAction()));

            foreach (var action in _currentTurn)
            {
                action.Move();
            }

            foreach (var action in _currentTurn)
            {
                action.CastSpell();
            }
        }

        private void Wait()
        {
            if (_characters.Any(c => !c.Animation.IsIdle()))
            {
                return;
            }

            _currentTurn.Clear();
            _isPlanningPhase = true;
        }
    }
}
