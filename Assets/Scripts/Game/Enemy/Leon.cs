using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leon : Enemy {
    public override Enemies Type => Enemies.LEON;

    protected override Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    protected override void Awake() {
        AbilityHandlers = new Dictionary<AbilityID, AbilityHandler>() {
            { AbilityID.LeonSwing, SwingHandler },
            { AbilityID.LeonRighteousFury, RighteousFuryHandler },
            { AbilityID.LeonRighteousDefense, RighteousDefenseHandler }
        };

        base.Awake();
    }

    protected void SwingHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("Swing");

        // Sound
        AM.PlayFromUnitWithDelay("LeonSwing", 0.5f, this);
    }

    protected void RighteousFuryHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("Shout");

        // Sound
        // AM.PlayFromUnitWithDelay("GruntSwing", 0.5f, this);

        // Effect
        GameObject shoutEffect = PM.leonShoutEffectPrefab;
        Vector3 offset = new Vector3(0f, -2f, 0f);
        Quaternion rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        GameObject shoutSparkles = Instantiate(shoutEffect, transform.position + offset, rotation);
        Destroy(shoutSparkles, 3f);
    }

    protected void RighteousDefenseHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("Smash");

        // Sound
        AM.PlayFromUnit("LeonRighteousDefenseWindup", this);

        // Effect
        StartCoroutine(Smash());

        GameObject smashEffect = PM.leonSmashEffectPrefab;
        Vector3 offset = new Vector3(0f, -2f, 0f);
        Quaternion rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        GameObject smashSwords = Instantiate(smashEffect, transform.position + offset, rotation);
        Destroy(smashSwords, 3f);
    }

    private IEnumerator Smash() {
        yield return new WaitForSeconds(0.75f);

        AM.PlayFromUnit("LeonRighteousDefenseSlam", this);

        GameObject smashEffect = PM.leonSmashEffectPrefab;
        Vector3 offset = new Vector3(0f, -2.4f, 0f);
        Quaternion rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        GameObject smash = Instantiate(smashEffect, transform.position + offset, rotation);
        Destroy(smash, .5f);
    }
}
