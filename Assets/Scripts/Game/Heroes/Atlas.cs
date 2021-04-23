using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atlas : Hero {
    public override Heroes Type { get; protected set; }

    protected override Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    // for mesh effects
    public GameObject weapon;

    protected override void InitializeHero() {
        Type = Heroes.ATLAS;
        RNG = new System.Random();

        MeshEffectApplicator.instance.ApplyAtlasAxeEffect(weapon);

        AbilityHandlers = new Dictionary<AbilityID, AbilityHandler>() {
            { AbilityID.AtlasSwing, SwingHandler },
            { AbilityID.AtlasThunderClap, ThunderClapHandler },
            { AbilityID.AtlasOverload, OverloadHandler },
            { AbilityID.AtlasLeap, LeapHandler },
            { AbilityID.AtlasBattleTrance, BattleTranceHandler },
            { AbilityID.AtlasThunderousFortitude, ThunderousFortitudeHandler },
            { AbilityID.AtlasRampage, RampageHandler }
        };
    }

    public void SwingHandler(AbilityID _ability, List<int> _affectedUnits, List<Vector3> _locationsData) {
        // Animation
        if (RNG.Next(2) == 0) {
            Anim.SetTrigger("Swing1");
        } else {
            Anim.SetTrigger("Swing2");
        }

        // Sound
        AM.PlayFromUnitWithDelay("AtlasSwing", 0.4f, this);
    }

    public void ThunderClapHandler(AbilityID _ability, List<int> _affectedUnits, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("ThunderClap");

        // Sound
        AM.PlayFromUnitWithDelay("AtlasThunderClap", 0.65f, this);
    }

    public void OverloadHandler(AbilityID _ability, List<int> _affectedUnits, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("Overload");

        // Effect
        GameObject explosionEffect = PM.overloadEffectPrefab;
        GameObject explosion = Instantiate(explosionEffect, transform, false);
        Destroy(explosion, 1f);

        // Sound
        AM.PlayFromUnit("AtlasOverload", this);
    }

    public void LeapHandler(AbilityID _ability, List<int> _affectedUnits, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("Leap");

        // Sound
    }

    public void BattleTranceHandler(AbilityID _ability, List<int> _affectedUnits, List<Vector3> _locationsData) {
        // Animation


        // Effect
        GameObject tranceEffect = PM.battleTranceEffectPrefab;
        GameObject effect = Instantiate(tranceEffect, transform, false);
        Destroy(effect, 2f);

        // Sound
        AM.PlayFromUnit("AtlasBattleTrance", this);
    }

    public void ThunderousFortitudeHandler(AbilityID _ability, List<int> _affectedUnits, List<Vector3> _locationsData) {
        // Animation

        // Effect

        // Sound
        AM.PlayFromUnit("AtlasThunderousFortitude", this);
    }

    public void RampageHandler(AbilityID _ability, List<int> _affectedUnits, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("Shout");

        // Effect

        // Sound
        AM.PlayFromUnit("AtlasRampageTrigger", this);

        if (HeroIsControlledByClient) {
            AM.PlayFromUnit("AtlasRampageLoop", this);
        }
    }
}
