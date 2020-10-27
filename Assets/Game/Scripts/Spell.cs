using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] string spellName;
    [SerializeField] int damage;
    [SerializeField] int targets;
    [SerializeField] int actionPointCost;
    [SerializeField] int cooldown;
    [SerializeField] string selfEffect;
    [SerializeField] string falvorText;
    [SerializeField] string notes;

    [SerializeField] Sprite thumbnail;
}
