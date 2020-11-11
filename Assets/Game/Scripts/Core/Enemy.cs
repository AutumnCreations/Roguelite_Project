using UnityEngine;

namespace Scripts.Core
{
    public class Enemy : MonoBehaviour
    {
        private static readonly System.Random Random = new System.Random();

        [SerializeField] public World World;

        private Character _character;

        private void Start()
        {
            _character = transform.GetComponent<Character>();
        }

        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            if (!_character.IsPlanningPhase || _character.Actions.Count > 0)
            {
                return;
            }

            var q = Random.Next(3) - 1;
            var r = Random.Next(3) - 1;
            if (q != r)
            {
                _character.Move(q, r);
            }
        }
    }
}
