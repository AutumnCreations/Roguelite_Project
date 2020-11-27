using Scripts.Items;
using UnityEngine;

namespace Scripts.Characters
{
    [CreateAssetMenu(fileName = "New NPC Type", menuName = "Characters/NPC Type")]
    public class NPCType : ScriptableObject
    {
        [SerializeField] private Spell[] spells;

        public string Name { get { return name; } }
        public Spell[] Spells { get { return spells; } }
    }
}
