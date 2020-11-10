using UnityEngine;

namespace Scripts.Items
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private Sprite thumbnail;
        [SerializeField] private string description;
        [SerializeField] private string flavorText;

        public Sprite Thumbnail { get { return thumbnail; } }
        public string Name { get { return name; } }
        public abstract string ColoredName { get; }
        public string Description { get { return description; } }
        public string FlavorText { get { return flavorText; } }

        public abstract string GetTooltipInfoText();
    }
}
