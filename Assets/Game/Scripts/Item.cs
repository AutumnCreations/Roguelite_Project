using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private string flavorText;
    [SerializeField] private int damage;
    [SerializeField] private int cooldown;
    [SerializeField] private Sprite thumbnail;

    public string Name { get { return name; } }
    public abstract string ColoredName { get; }

    public string FlavorText { get { return flavorText; } }
    public int Damage { get { return damage; } }
    public int Cooldown { get { return cooldown; } }
    public Sprite Thumbnail { get { return thumbnail; } }

    public abstract string GetTooltipInfoText();
}
