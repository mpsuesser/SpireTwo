using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public static GameState instance;

    // temp
    private int stateNum = 0;
    public int GetStateNum => stateNum++;

    // To determine if the game is started and therefore locked to new players.
    public bool HasStarted {
        private set;
        get;
    }

    public int PlayersConnected {
        private set;
        get;
    }

    public Transform heroesParentObject;
    public List<Hero> HeroesSpawned {
        private set;
        get;
    }

    public Transform enemiesParentObject;
    public List<Enemy> EnemiesSpawned {
        private set;
        get;
    }

    public Hero P1 {
        get => HeroesSpawned.Count < 1 ? null : HeroesSpawned[0];
    }

    public Hero P2 {
        get => HeroesSpawned.Count < 2 ? null : HeroesSpawned[1];
    }

    public DungeonDetails DD;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one GameState instance in scene!");
            return;
        }

        instance = this;
        #endregion

        HasStarted = false;
        PlayersConnected = 0;
        HeroesSpawned = new List<Hero>();
        EnemiesSpawned = new List<Enemy>();
    }

    public void StartGame() {
        HasStarted = true;
    }

    public GameObject SpawnHero(int _unitId, int _ownerId, int _heroNum, Vector3 _location, Vector3 _eulerAngles, float _health, float _maxHealth, float _mana, float _maxMana) {
        // Get the prefab associated with the hero specified.
        GameObject heroPrefab = PrefabManager.instance.heroPrefabs[(Heroes)_heroNum];

        // Instantiate the new hero.
        GameObject g = Instantiate(
            heroPrefab,
            _location,
            Quaternion.Euler(_eulerAngles)
        );

        // Set parent to our GameObject which will contain all active heroes.
        g.transform.SetParent(heroesParentObject, false);

        // Get the hero component.
        Hero h = g.GetComponent<Hero>();

        // Initialize
        h.Initialize(_unitId, _ownerId, _health, _maxHealth, _mana, _maxMana);

        // Add the new hero to our list of active ones.
        HeroesSpawned.Add(h);

        // If we own this unit, set it in our HeroController.
        if (Client.instance.myId == _ownerId) {
            HeroController.instance.SetHero(h);
        }

        // Return the GameObject corresponding to the newly spawned hero.
        return g;
    }

    public void UpdatePositionOfHero(int _unitId, Vector3 _pos) {
        Hero h = GetHeroByUnitID(_unitId);
        if (h == null) {
            return;
        }

        h.UpdatePosition(_pos);
    }

    public void UpdateStatusOfHero(int _unitId, float _health, float _mana) {
        Hero h = GetHeroByUnitID(_unitId);
        if (h == null) {
            return;
        }

        h.SetHealth(_health);
        h.SetMana(_mana);
    }

    public GameObject EnemySpawned(int _enemyId, Enemies _type, EnemyStateID _stateId, float _health, float _maxHealth, Vector3 _pos, Quaternion _rotation) {
        Debug.Log($"Enemy spawned with ID {_enemyId}");

        // Get the prefab associated with the enemy type given.
        GameObject enemyPrefab = PrefabManager.instance.enemyPrefabs[_type];

        // Instantiate the enemy with a random rotation around the Y axis.
        GameObject g = Instantiate(enemyPrefab, _pos, _rotation);

        // Group the new enemy object into our enemies GameObject.
        g.transform.SetParent(enemiesParentObject, false);

        // Initialize the Enemy with the given ID and max health given.
        Enemy e = g.GetComponent<Enemy>();
        e.Initialize(_enemyId, _stateId, _health, _maxHealth);

        // Add it to our running list of enemies.
        EnemiesSpawned.Add(e);

        // Return our newly instantiated enemy game object.
        return g;
    }

    public void UpdatePositionOfEnemy(int _enemyId, Vector3 _pos, Quaternion _rotation) {
        Enemy e = GetEnemyByUnitID(_enemyId);
        if (e == null) {
            return;
        }

        e.UpdatePositionAndRotation(_pos, _rotation);
    }

    public void UpdateStatusOfEnemy(int _enemyId, float _health) {
        Enemy e = GetEnemyByUnitID(_enemyId);
        if (e == null) {
            return;
        }

        e.SetHealth(_health);
    }

    public void DestroyEnemy(int _enemyId) {
        Enemy e = GetEnemyByUnitID(_enemyId);
        if (e == null) {
            return;
        }

        Destroy(e.gameObject, Constants.ENEMY_DEATH_DESTROY_TIME);
    }

    #region Helper Functions
    /* public Enemy GetEnemyByUnitID(int _enemyId) {
        foreach (Enemy _e in EnemiesSpawned) {
            if (_e.ID == _enemyId) {
                return _e;
            }
        }

        Debug.Log($"Enemy with ID {_enemyId} could not be found!");
        return null;
    } */

    public Hero GetHeroByUnitID(int _unitId) => HeroesSpawned.Find(h => h.ID == _unitId);
    public Enemy GetEnemyByUnitID(int _unitId) => EnemiesSpawned.Find(e => e.ID == _unitId);
    public Unit GetUnitByUnitID(int _unitId) {
        Unit u = GetHeroByUnitID(_unitId);
        if (u == null) {
            u = GetEnemyByUnitID(_unitId);
        }

        return u;
    }
    #endregion
}
