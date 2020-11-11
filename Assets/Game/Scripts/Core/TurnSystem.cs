using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Core
{
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] private Character _player;
        [SerializeField] private List<Character> _enemies;

        private IEnumerable<Character> _characters;

        private bool _isRunning;

        private void Awake()
        {
            _characters = new[] { _player }.Concat(_enemies);
            Reset();
        }

        private void Update()
        {
            if (_isRunning)
            {
                if (_characters.Sum(e => e.Actions.Count) == 0)
                {
                    Reset();
                }
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                _isRunning = true;
                foreach (var character in _characters)
                {
                    character.IsPlanningPhase = false;
                }

                NextTurn();
            }
        }

        private void Reset()
        {
            foreach (var character in _characters)
            {
                character.IsPlanningPhase = true;
            }

            _isRunning = false;
        }

        private void NextTurn()
        {
            foreach (var character in _characters)
            {
                StartCoroutine(RunActions(character));
            }
        }

        private static IEnumerator RunActions(Character character)
        {
            foreach (var action in character.Actions)
            {
                yield return action.Act();
            }

            character.Actions.Clear();
        }
    }
}
