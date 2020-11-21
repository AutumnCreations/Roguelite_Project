using Scripts.Characters;
using Scripts.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class SpellSlots : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private SpellButton spellButtonReference;
        [SerializeField] private List<SpellButton> spellButtons;

        private void Awake()
        {
            var character = player.GetComponent<Character>();
            var slots = character.spells.Count;
            for (int i = 0; i < slots; i++)
            {
                var spellButton = Instantiate(spellButtonReference, gameObject.transform);
                spellButton.spell = character.spells[i];
                spellButton.player = player;
                spellButtons.Add(spellButton);
            }
        }

    }
}
