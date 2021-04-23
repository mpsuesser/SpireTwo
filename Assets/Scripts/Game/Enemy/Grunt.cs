using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy {
    public override Enemies Type => Enemies.GRUNT;

    protected override Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    protected override void Awake() {
        AbilityHandlers = new Dictionary<AbilityID, AbilityHandler>() {
            { AbilityID.GruntSwing, SwingHandler }
        };

        base.Awake();
    }

    protected void SwingHandler(AbilityID _abilityID, List<int> _affectedUnitIDs, List<Vector3> _locationsData) {
        // Animation
        String[] swingTriggers = { "Swing1", "Swing2", "Swing3", "Swing4" };
        string trigger = Util.GetRandomString(swingTriggers, RNG);
        Anim.SetTrigger(trigger);

        // Sound
        AM.PlayFromUnitWithDelay("GruntSwing", 0.5f, this);
    }
}
