using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffBar : BuffBar {
    private Enemy root;

    void Start() {
        root = gameObject.GetComponentInParent<Enemy>();
        if (root == null) {
            Debug.Log("Could not find Enemy component in parent! Destroying!");
            Destroy(gameObject);
        }

        root.BuffApplied += CreateBuff;
        root.BuffPurged += PurgeBuff;
    }

    void OnDestroy() {
        if (root != null) {
            root.BuffApplied -= CreateBuff;
            root.BuffPurged -= PurgeBuff;
        }
    }
}
