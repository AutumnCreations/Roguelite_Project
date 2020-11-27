using UnityEngine;

namespace Scripts.Items
{
    [CreateAssetMenu(fileName = "New Spell Type", menuName = "Items/Spell Type")]
    public class SpellType : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private Color textColour;

        public string Name { get { return name; } }
        public Color TextColour { get { return textColour; } }
    }
}
