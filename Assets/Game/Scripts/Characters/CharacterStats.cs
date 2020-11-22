using System.Collections.Generic;
using Scripts.Items;
using UnityEngine;

namespace Scripts.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Location")]
        [SerializeField] public int Q;
        [SerializeField] public int R;

        [Header("Combat")]
        [SerializeField] public float Health = 10f;
        [SerializeField] public List<Spell> Spells;
    }
}
