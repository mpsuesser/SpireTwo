using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpellName", menuName = "ScriptableObjects/SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    public string abilityName;
    public AbilityID abilityID;
    public Sprite icon;

    [TextArea(15,20)]
    public string description;
}
