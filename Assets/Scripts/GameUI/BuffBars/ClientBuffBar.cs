using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBuffBar : BuffBar {
    private Hero current;

    void Start() {
        current = null;

        HeroController.instance.HeroSet += HeroUpdated;
    }

    public void HeroUpdated(Hero _h) {
        ClearExisting();

        _h.BuffApplied += CreateBuff;
        _h.BuffPurged += PurgeBuff;

        current = _h;
    }

    void OnDestroy() {
        ClearExisting();

        if (HeroController.instance != null) {
            HeroController.instance.HeroSet -= HeroUpdated;
        }
    }

    private void ClearExisting() {
        if (current != null) {
            current.BuffApplied -= CreateBuff;
            current.BuffPurged -= PurgeBuff;
        }
    }
}
