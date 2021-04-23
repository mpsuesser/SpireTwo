using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBarManager : MonoBehaviour
{
    public static AbilityBarManager instance;

    public AbilityBar atlasAbilityBar;
    public AbilityBar priestessAbilityBar;

    public AbilityBar CurrentBar { get; private set; }

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one AbilityBarManager instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    void Start() {
        CurrentBar = null;
        HeroController.instance.HeroSet += SelectedHeroUpdated;
    }

    public void SelectedHeroUpdated(Hero _h) {
        if (_h.Type == Heroes.ATLAS) {
            CurrentBar = atlasAbilityBar;
            atlasAbilityBar.gameObject.SetActive(true);
        } else if (_h.Type == Heroes.PRIESTESS) {
            CurrentBar = priestessAbilityBar;
            priestessAbilityBar.gameObject.SetActive(true);
        }
    }

    // Just to make sure we never have two ability bars up at once.
    private void DeactivateAll() {
        atlasAbilityBar.gameObject.SetActive(false);
        priestessAbilityBar.gameObject.SetActive(false);
    }
}
