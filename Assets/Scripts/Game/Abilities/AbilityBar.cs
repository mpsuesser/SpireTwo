using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour
{
    public Ability[] abilities;
    public InputCatcher inputCatcher;

    private void Start() {
        for (int i = 0; i < abilities.Length; i++) {
            // TODO: Update to pull from settings instead.
            abilities[i].Initialize(Constants.ABILITY_HOTKEYS[i]);
            // TODO: Temp
            abilities[i].SetReady();
            abilities[i].OnAbilityPress += AbilityPressed;
        }

        // Register our new abilities to have their hotkeys caught if pressed.
        inputCatcher.RegisterAbilities(abilities);
    }

    public void AbilityPressed(Ability _a) {
        for (int i = 0; i < abilities.Length; i++) {
            abilities[i].PutOnGCD();
        }
    }

    public Ability GetAbilityByID(AbilityID _abilityID) {
        for (int i = 0; i < abilities.Length; i++) {
            if (abilities[i].ID == _abilityID) {
                return abilities[i];
            }
        }

        return null;
    }
}
