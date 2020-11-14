using Scripts.Characters;
using Scripts.Turns.Actions;
using UnityEngine;

namespace Scripts.Control
{
    [RequireComponent(typeof(Character))]
    public abstract class AbstractControl : MonoBehaviour
    {
        public abstract TurnAction NextAction();
    }
}
