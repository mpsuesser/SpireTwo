using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
    public static PrefabManager instance;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one PrefabManager instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    // Heroes
    public GameObject atlasPrefab;
    public GameObject priestessPrefab;

    public Dictionary<Heroes, GameObject> heroPrefabs;

    // Enemies
    public GameObject gruntPrefab;
    public GameObject spellslingerPrefab;
    public GameObject leonPrefab;
    public GameObject frostyPrefab;

    public Dictionary<Enemies, GameObject> enemyPrefabs;

    // Effects
    public GameObject overloadEffectPrefab;
    public GameObject thunderousFortitudeEffectPrefab;
    public GameObject battleTranceEffectPrefab;
    public GameObject leonSmashEffectPrefab;
    public GameObject leonShoutEffectPrefab;
    public GameObject frostySpiritStrikeEffectPrefab;
    public GameObject frostySpiritBombEffectPrefab;
    public GameObject frostySpiritBombingPhaseEffectPrefab;

    void Start() {
        heroPrefabs = new Dictionary<Heroes, GameObject>() {
            { Heroes.ATLAS, atlasPrefab },
            { Heroes.PRIESTESS, priestessPrefab }
        };

        enemyPrefabs = new Dictionary<Enemies, GameObject>() {
            { Enemies.GRUNT, gruntPrefab },
            { Enemies.SPELLSLINGER, spellslingerPrefab },
            { Enemies.LEON, leonPrefab },
            { Enemies.FROSTY, frostyPrefab }
        };

        PreloadManager.instance.Ready(this);
    }
}
