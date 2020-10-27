using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Items/Spell")]
public class Spell : Item
{
    [SerializeField] private SpellType spellType;
    [SerializeField] private string spellText = "Does something";

    //[SerializeField] private int targets;
    //[SerializeField] private int actionPointCost;
    //[SerializeField] private string selfEffect;
    //[SerializeField] private string notes;
    //[SerializeField] private Sprite thumbnail;

    public SpellType SpellType { get { return spellType; } }

    public override string ColoredName
    {
        get
        {
            string hexColour = ColorUtility.ToHtmlStringRGB(spellType.TextColour);
            return $"<color=#{hexColour}>{Name}</color>";
        }
    }



    public override string GetTooltipInfoText()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(SpellType.Name).AppendLine();
        builder.Append(spellText).AppendLine();
        builder.Append("Deals ").Append(Damage).Append(" Damage").AppendLine();
        builder.Append(FlavorText);

        return builder.ToString();
    }
}
