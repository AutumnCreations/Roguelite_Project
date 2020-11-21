using Scripts.Characters;
using Scripts.Control;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class SpellSlots : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private SpellButton spellButtonReference;
        [SerializeField] private List<SpellButton> spellButtons;

        private void Start()
        {
            var character = player.GetComponent<Character>();

            foreach (var spell in character.Stats.spells)
            {
                var spellButton = Instantiate(spellButtonReference, gameObject.transform);
                spellButton.spell = spell;
                spellButton.player = player;
                spellButtons.Add(spellButton);
            }
        }
    }
}
