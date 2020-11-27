using UnityEngine;

namespace Scripts.Characters
{
    [CreateAssetMenu(fileName = "New NPC", menuName = "Characters/NPC")]
    public class NPC : ScriptableObject
    {
        [SerializeField] private NPCType type;
        [SerializeField] private string NPCName;
        [SerializeField] private int health;

    }
}
