using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour {
    public SpellData spellData;

    private Image icon;
    private GameObject tooltipContent;
    private Text tooltipText;

    public void Awake() {
        icon = transform.Find("Icon").gameObject.GetComponent<Image>();
        tooltipContent = transform.Find("Tooltip").gameObject;
        tooltipText = tooltipContent.GetComponentInChildren<Text>();
    }

    public void Start() {
        icon.sprite = spellData.icon;
        tooltipText.text = $"{spellData.name}\n\n{spellData.description}";

        tooltipContent.SetActive(false);
    }

    public void HoverEnter() {
        tooltipContent.SetActive(true);
    }

    public void HoverExit() {
        tooltipContent.SetActive(false);
    }
}
