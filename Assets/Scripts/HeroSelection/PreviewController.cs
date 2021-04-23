using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewController : MonoBehaviour
{
    public GameObject content;
    public Text previewText;

    public GameObject modelContainer;
    public GameObject abilitiesContainer;
    private Dictionary<Heroes, GameObject> modelMap;
    private Dictionary<Heroes, GameObject> abilitiesMap;

    public void Start() {
        modelMap = new Dictionary<Heroes, GameObject>();
        abilitiesMap = new Dictionary<Heroes, GameObject>();

        foreach(Heroes hero in Names.Hero.Keys) {
            modelMap.Add(hero, modelContainer.transform.Find(Names.Hero[hero]).gameObject);
            abilitiesMap.Add(hero, abilitiesContainer.transform.Find(Names.Hero[hero]).gameObject);
        }

        ClearPreviews();
    }

    public void ClearPreviews() {
        foreach (GameObject model in modelMap.Values) {
            model.SetActive(false);
        }

        foreach (GameObject abilityIcons in abilitiesMap.Values) {
            abilityIcons.SetActive(false);
        }

        previewText.text = "";

        content.SetActive(false);
    }

    public void HeroSelected(Heroes _hero) {
        modelMap[_hero].SetActive(true);
        abilitiesMap[_hero].SetActive(true);

        previewText.text = Names.Hero[_hero];

        content.SetActive(true);
    }
}
