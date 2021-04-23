using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frosty : Enemy {
    public override Enemies Type => Enemies.FROSTY;

    protected override Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    protected override void Awake() {
        AbilityHandlers = new Dictionary<AbilityID, AbilityHandler>() {
            { AbilityID.FrostySwing, SwingHandler },
            { AbilityID.FrostySpiritStrike, SpiritStrikeHandler },
            { AbilityID.FrostySpiritBombStartPhase, SpiritBombStartPhaseHandler },
            { AbilityID.FrostySpiritBombEndPhase, SpiritBombEndPhaseHandler },
            { AbilityID.FrostySpiritBombSpawn, SpiritBombSpawnHandler },
            { AbilityID.FrostySpiritBombExplode, SpiritBombExplodeHandler }
        };

        base.Awake();
    }

    protected void SwingHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        String[] swingTriggers = { "Swing1", "Swing2" };
        string trigger = Util.GetRandomString(swingTriggers, RNG);
        Anim.SetTrigger(trigger);

        // Sound
        // AM.PlayFromUnitWithDelay("GruntSwing", 0.5f, this);
    }

    protected void SpiritStrikeHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Checks
        if (_affectedUnitIDs.Count != _locationsData.Count) {
            Debug.Log("Affected unit count did not match up with locations count!");
            return;
        }

        // Animation
        Anim.SetTrigger("SpiritStrike");

        // Effect
        GameObject effect = PrefabManager.instance.frostySpiritStrikeEffectPrefab;
        foreach (Vector3 location in _locationsData) {
            Instantiate(effect, Util.GetGround(location), Quaternion.identity);
        }

        // Sound


    }

    private GameObject spiritBombPhaseEffect;
    protected void SpiritBombStartPhaseHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        Anim.SetBool("SpiritBombing", true);

        // Effect
        GameObject effect = PrefabManager.instance.frostySpiritBombingPhaseEffectPrefab;
        spiritBombPhaseEffect = Instantiate(effect, Util.GetGround(transform.position), transform.rotation);

        // Sound

    }

    protected void SpiritBombEndPhaseHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        Anim.SetBool("SpiritBombing", false);

        // Effect
        if (spiritBombPhaseEffect != null) {
            Destroy(spiritBombPhaseEffect);
            spiritBombPhaseEffect = null;
        }

        // Sound

    }

    protected void SpiritBombSpawnHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation


        // Effect
        GameObject effect = PrefabManager.instance.frostySpiritBombEffectPrefab;
        Instantiate(effect, Util.GetGround(_locationsData[0]), Quaternion.identity);

        // Sound

    }

    protected void SpiritBombExplodeHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation


        // Effect


        // Sound

    }
}
