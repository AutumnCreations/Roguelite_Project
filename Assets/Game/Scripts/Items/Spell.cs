﻿using System.Text;
using UnityEngine;

namespace Scripts.Items
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "Items/Spell")]
    public class Spell : Item
    {
        [SerializeField] private SpellType spellType;
        [SerializeField] private int actionPointCost;
        [SerializeField] private int damage;
        [SerializeField] private int range;
        [SerializeField] private int cooldown;

        [SerializeField] public GameObject spellEffect;

        public int ActionPointCost { get { return actionPointCost; } }
        public int Damage { get { return damage; } }
        public int Range { get { return range; } }
        public int Cooldown { get { return cooldown; } }
        public SpellType SpellType { get { return spellType; } }

        //[SerializeField] private int targets;
        //[SerializeField] private string selfEffect;
        //[SerializeField] private string notes;
        //[SerializeField] private Sprite thumbnail;

        public override string GetTooltipInfoText()
        {
            StringBuilder builder = new StringBuilder();

            //builder.Append(SpellType.Name).AppendLine();
            builder.Append(Description).AppendLine();
            builder.Append("AP: ").Append(ActionPointCost).AppendLine();
            builder.Append("Damage: ").Append(Damage).AppendLine();
            builder.Append("Range: ").Append(Range).AppendLine();
            builder.Append("Cooldown: ").Append(Cooldown).Append(" Turn(s)").AppendLine();
            builder.Append("<i>" + FlavorText + "</i>");

            return builder.ToString();
        }

        public override string ColoredName
        {
            get
            {
                string hexColour = ColorUtility.ToHtmlStringRGB(spellType.TextColour);
                return $"<color=#{hexColour}>{Name}</color>";
            }
        }
    }
}
