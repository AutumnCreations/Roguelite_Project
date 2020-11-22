using Scripts.Characters;
using Scripts.Turns.Actions;
using UnityEngine;

namespace Scripts.Control
{
    [RequireComponent(typeof(Character))]
    public abstract class AbstractControl : MonoBehaviour
    {
        public Character Character { get; private set; }

        protected void Awake()
        {
            Character = transform.GetComponent<Character>();
        }

        public abstract TurnAction NextAction();
    }
}
