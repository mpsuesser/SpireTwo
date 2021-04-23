using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
    public static GameMaster instance;

    private GameState GS;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one GameMaster instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    void Start() {
        GS = GameState.instance;
    }

    void Update() {

    }
}
