using System.Collections;
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

        private TurnState _state;
        private List<TurnAction> _currentTurn;

        private void Awake()
        {
            _state = TurnState.Plan;
            _currentTurn = new List<TurnAction>();
        }

        private void Start()
        {
            StartCoroutine(Execute());
        }

        private void Update()
        {
            if (_state != TurnState.Plan)
            {
                return;
            }

            var playerAction = _player.NextAction();
            if (playerAction == null)
            {
                return;
            }

            _currentTurn.Add(playerAction);
            _currentTurn.AddRange(_enemies.Select(e => e.NextAction()));

            _state = TurnState.Ready;
        }

        private IEnumerator Execute()
        {
            while (true)
            {
                if (_state != TurnState.Ready)
                {
                    yield return null;
                    continue;
                }

                _state = TurnState.Act;

                foreach (var action in _currentTurn)
                {
                    StartCoroutine(action.Act());
                }

                while (!_currentTurn.All(r => r.Completed))
                {
                    yield return null;
                }

                _currentTurn.Clear();
                _state = TurnState.Plan;
            }
        }
    }
}
