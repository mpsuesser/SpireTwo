using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    public static HeroController instance;

    public Hero H { get; private set; }

    // Our event hook for when a hero is spawned/set.
    public event Action<Hero> HeroSet;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one HeroController instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    void Start() {
        H = null;
    }

    public void SetHero(Hero _h) {
        H = _h;

        // Since CursorManager is a preloaded script and can't easily subscribe to this scene-specific component, we'll let it know here that a new hero has been set.
        CursorManager.instance.SetHeroCursor(_h.Type);

        // Let other components that have subscribed to this event know that a hero has been set.
        if (HeroSet != null) {
            HeroSet(H);
        }
    }
}
