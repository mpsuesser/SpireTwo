using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatusBars : MonoBehaviour
{
    public Text healthText;
    public Text manaText;

    public CryptUI.Scripts.ResourceBarController healthController;
    public CryptUI.Scripts.ResourceBarController manaController;

    public void Start() {
        if (HeroController.instance.H == null) {
            HeroController.instance.HeroSet += RegisterHooks;
            return;
        }

        RegisterHooks(HeroController.instance.H);
    }

    public void RegisterHooks(Hero _h) {
        _h.UpdatedStatus += UpdateBars;

        // Call it manually once for the initial setting of values.
        UpdateBars(_h.Health, _h.Mana);
    }

    public void UpdateBars(float _health, float _mana) {
        float maxHealth = HeroController.instance.H.MaxHealth;
        float maxMana = HeroController.instance.H.MaxMana;
        float healthPercentage = _health / maxHealth;
        float manaPercentage = _mana / maxMana;

        healthController.ApplyValue(healthPercentage);
        manaController.ApplyValue(manaPercentage);

        string fmt = "000.##";
        healthText.text = $"{Mathf.RoundToInt(_health).ToString(fmt)} / {Mathf.RoundToInt(maxHealth).ToString(fmt)}";
        manaText.text = $"{Mathf.RoundToInt(_mana).ToString(fmt)} / {Mathf.RoundToInt(maxMana).ToString(fmt)}";
    }
}
