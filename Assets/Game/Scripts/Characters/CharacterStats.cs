using System.Collections.Generic;
using Scripts.Items;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Location")]
        [SerializeField] public int Q;
        [SerializeField] public int R;
        [SerializeField] public WorldTile lastTile = null;

        [Header("Combat")]
        [SerializeField] private float health = 10f;
        [SerializeField] public List<Spell> spells;

        public float Health
        {
            get => health;
            set => health = value;
        }
    }
}
