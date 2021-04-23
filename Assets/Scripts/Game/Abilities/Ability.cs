using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ability : MonoBehaviour {
    public SpellData spellData;
    public AbilityID ID => spellData.abilityID;

    public event Action<Ability> OnAbilityPress;

    public KeyCode Hotkey { get; private set; }

    public float RecentCooldownMax { get; private set; } // to tell us if the GCD or actual cooldown were used when setting the cooldown
    public bool OnCooldown { get => cooldownRemaining > 0f; }

    private Transform iconContainer;
    private Image positiveIcon;
    private Image negativeIcon;
    private Text hotkeyText;
    private Text cooldownText;
    private Color negativeColor;

    private float cooldownRemaining;
    private bool cooldownRoutineRunning;
    private IEnumerator cooldownCoroutine;

    public void Awake() {
        iconContainer = transform.Find("Icons");
        positiveIcon = iconContainer.Find("Positive").gameObject.GetComponent<Image>();
        negativeIcon = iconContainer.Find("Negative").gameObject.GetComponent<Image>();
        cooldownText = iconContainer.Find("Cooldown").gameObject.GetComponent<Text>();
        hotkeyText = transform.Find("Hotkey").gameObject.GetComponent<Text>();

        negativeColor = new Color(0.094f, 0.094f, 0.094f, 0.58f);
    }

    public void Start() {
        positiveIcon.sprite = spellData.icon;
        negativeIcon.sprite = spellData.icon;
        negativeIcon.color = negativeColor;
    }

    public void Initialize(KeyCode _kc) {
        Hotkey = _kc;

        string hkText;

        // Leave the mousebutton ones using preset hotkey text
        if (_kc == KeyCode.Mouse0) {
            hkText = "M1";
        } else if (_kc == KeyCode.Mouse1) {
            hkText = "M2";
        } else {
            hkText = _kc.ToString();
        }

        hotkeyText.text = hkText;
    }

    // To be overrided in some abilities that will inherit this class and are not normal cooldown spells but instead are e.g. activated via procs.
    public virtual void SetReady() {
        // If this ability has been brought off cooldown by a proc or something, let's stop the running coroutine that updates the cooldown status.
        if (cooldownRoutineRunning != false) {
            StopCoroutine(cooldownCoroutine);
        }

        cooldownRoutineRunning = false;

        // Our indication that the ability is not on cooldown.
        cooldownRemaining = -1f;

        // Empty the cooldown text over the icon.
        cooldownText.text = "";

        // Fill the positive icon.
        positiveIcon.fillAmount = 1f;
    }

    public virtual void PressedClient() {
        if (OnCooldown) {
            return;
        }

        ClientSend.AbilityPressed(HeroController.instance.H.OwnerID, spellData.abilityID);
    }

    public virtual void PressedServer(float _cooldown) {
        PutOnCooldown(_cooldown);

        if (OnAbilityPress != null) {
            OnAbilityPress(this);
        }
    }

    public virtual void PutOnGCD() {
        float gcdTime = Constants.GCD_TIME;

        if (OnCooldown && cooldownRemaining > gcdTime) {
            return;
        }

        cooldownRemaining = gcdTime;
        RecentCooldownMax = gcdTime;

        ActivateCooldownCoroutine();
    }

    public virtual void PutOnCooldown(float _cooldown) {
        // Set the remaining cooldown to the max.
        cooldownRemaining = _cooldown;
        RecentCooldownMax = _cooldown;

        ActivateCooldownCoroutine();
    }

    private void ActivateCooldownCoroutine() {
        if (!cooldownRoutineRunning && gameObject.activeSelf) {
            cooldownCoroutine = UpdateCooldownStatus();
            StartCoroutine(cooldownCoroutine);
        }
    }

    private IEnumerator UpdateCooldownStatus() {
        cooldownRoutineRunning = true;
        // Run this loop until we get real close to 0f. Stop a bit before just to avoid any weird interactions with informally having our OnCooldown return true before we've set things up to indicate it's ready.
        while (cooldownRemaining > 0.05f) {
            // Update the remaining time on cooldown.
            cooldownRemaining -= Time.deltaTime;

            // Update the fill amount of the positive icon.
            positiveIcon.fillAmount = 1f - (cooldownRemaining / RecentCooldownMax);

            // Round the remaining cooldown up to nearest int and display it as cooldown text.
            cooldownText.text = Mathf.CeilToInt(cooldownRemaining).ToString();

            // Yield until the next loop
            yield return null;
        }

        // Cooldown is over, let's reset the ability status to indicate it's ready for use.
        cooldownRoutineRunning = false;
        SetReady();
    }
}
