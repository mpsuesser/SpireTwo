using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priestess : Hero {
    public override Heroes Type { get; protected set; }

    protected override Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    protected override void InitializeHero() {
        Type = Heroes.PRIESTESS;

        AbilityHandlers = new Dictionary<AbilityID, AbilityHandler>() {

        };
    }
}
