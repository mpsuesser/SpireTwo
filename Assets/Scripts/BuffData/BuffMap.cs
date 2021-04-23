using UnityEngine;
using System.Collections.Generic;

public class BuffMap : MonoBehaviour {
    #region Singleton
    public static BuffMap instance;
    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one BuffMap instance in scene!");
            return;
        }

        instance = this;
    }
    #endregion

    public BuffData[] buffDataObjects;

    public BuffData GetBuffDataForBuffID(BuffID _buffId) {
        foreach(BuffData bd in buffDataObjects) {
            if (bd.buffID == _buffId) {
                return bd;
            }
        }

        return null;
    }
}