using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellslinger : Enemy {
    public override Enemies Type => Enemies.SPELLSLINGER;

    protected override Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    protected override void Awake() {
        AbilityHandlers = new Dictionary<AbilityID, AbilityHandler>() {
            { AbilityID.SpellslingerFireballCastStart, FireballCastStartHandler },
            { AbilityID.SpellslingerFireballShoot, FireballShootHandler },
            { AbilityID.SpellslingerSwing, SwingHandler }
        };

        base.Awake();
    }

    protected void FireballCastStartHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Cast bar
        StartCast(_abilityID, 3f);

        // Animation
        String[] castTriggers = { "Cast1", "Cast2", "Cast3" };
        string trigger = Util.GetRandomString(castTriggers, RNG);
        Anim.SetTrigger(trigger);

        // Sound
        AM.PlayFromUnit("SpellslingerFireballCasting", this);
    }

    protected void FireballShootHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        Anim.SetBool("Casting", false); // This is set to true on StateEnter from CastingBehaviour.cs

        // Sound
        AM.PlayFromUnit("SpellslingerFireballCastFinished", this);

        // Effect
        PC.ShootFireballFromSkeletonToHero(this, HeroController.instance.H);
    }

    protected void SwingHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        Anim.SetTrigger("Swing");

        // Sound
        // AM.PlayFromUnitWithDelay("GruntSwing", 0.5f, this);
    }
}
