using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuffName", menuName = "ScriptableObjects/BuffData", order = 1)]
public class BuffData : ScriptableObject {
    public string buffName;
    public BuffID buffID;
    public Sprite icon;

    [TextArea(15, 20)]
    public string description;
}
