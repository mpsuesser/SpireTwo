using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientReceive : MonoBehaviour {
    public static void WelcomeToServer(Packet _packet) {
        int _myId = _packet.ReadInt();
        bool _slotFree = _packet.ReadBool();

        Debug.Log($"[WelcomeToServer] Received. Slot free: {_slotFree}");
        Client.instance.myId = _myId;

        Debug.Log("[WelcomeToServer] Establishing UDP connection...");
        Client.instance.udp.Connect();

        Debug.Log("[WelcomeToServer] UDP connection should be good to go.");

        if (_slotFree) {
            Debug.Log("[WelcomeToServer] Opening HeroSelection scene.");
            SceneMaster.instance.LoadHeroSelection();
        } else {
            Debug.Log("[WelcomeToServer] Opening main scene as spectator since there was no slot free.");
            SceneMaster.instance.LoadGameWorld();
        }
    }

    public static void PlayerDisconnected(Packet _packet) {
        int _id = _packet.ReadInt();

        Debug.Log($"[PlayerDisconnected] ID: {_id}");
    }

    public static void SelectionAccepted(Packet _packet) {
        SceneMaster.instance.DungeonPreload();
    }

    public static void HeroSpawned(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _unitId = _packet.ReadInt();
        int _ownerId = _packet.ReadInt();
        int _heroNum = _packet.ReadInt();
        Vector3 _location = _packet.ReadVector3();
        Vector3 _eulerAngles = _packet.ReadVector3();
        float _health = _packet.ReadFloat();
        float _maxHealth = _packet.ReadFloat();
        float _mana = _packet.ReadFloat();
        float _maxMana = _packet.ReadFloat();

        GameState.instance.SpawnHero(_unitId, _ownerId, _heroNum, _location, _eulerAngles, _health, _maxHealth, _mana, _maxMana);
    }

    public static void HeroPositionUpdate(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _unitId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameState.instance.UpdatePositionOfHero(_unitId, _position);
    }

    public static void HeroStatusUpdate(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _unitId = _packet.ReadInt();
        float _health = _packet.ReadFloat();
        float _mana = _packet.ReadFloat();

        GameState.instance.UpdateStatusOfHero(_unitId, _health, _mana);
    }

    public static void EnemySpawned(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _enemyId = _packet.ReadInt();
        Enemies _type = (Enemies)_packet.ReadInt();
        EnemyStateID _stateId = (EnemyStateID)_packet.ReadInt();
        float _health = _packet.ReadFloat();
        float _maxHealth = _packet.ReadFloat();
        Vector3 _pos = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameState.instance.EnemySpawned(_enemyId, _type, _stateId, _health, _maxHealth, _pos, _rotation);
    }

    public static void EnemyPositionUpdate(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _enemyId = _packet.ReadInt();
        Vector3 _pos = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameState.instance.UpdatePositionOfEnemy(_enemyId, _pos, _rotation);
    }

    public static void EnemyStatusUpdate(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _enemyId = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameState.instance.UpdateStatusOfEnemy(_enemyId, _health);
    }

    public static void EnemyKilled(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _enemyId = _packet.ReadInt();

        GameState.instance.DestroyEnemy(_enemyId);
    }

    public static void AbilityFired(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _unitId = _packet.ReadInt();
        AbilityID _abilityID = (AbilityID)_packet.ReadInt();
        float _cooldown = _packet.ReadFloat();

        int _affectedUnitCount = _packet.ReadInt();
        List<int> _affectedUnitIds = null;
        if (_affectedUnitCount > 0) {
            _affectedUnitIds = new List<int>();
            for (int i = 0; i < _affectedUnitCount; i++) {
                _affectedUnitIds.Add(_packet.ReadInt());
            }
        }

        int _locationsCount = _packet.ReadInt();
        List<Vector3> _locations = null;
        if (_locationsCount > 0) {
            _locations = new List<Vector3>();
            for (int i = 0; i < _locationsCount; i++) {
                _locations.Add(_packet.ReadVector3());
            }
        }

        Hero h = GameState.instance.GetHeroByUnitID(_unitId);
        if (h == null) {
            Debug.Log("Hero was null on AbilityFired event! Doing nothing!");
            return;
        }

        h.HandleAbility(_abilityID, _affectedUnitIds, _locations, _cooldown);
    }

    public static void EnemyAbilityFired(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _enemyId = _packet.ReadInt();
        AbilityID _ability = (AbilityID)_packet.ReadInt();
        Debug.Log($"Ability fired: {_ability}");

        int _affectedUnitCount = _packet.ReadInt();
        List<int> _affectedUnitIds = null;
        if (_affectedUnitCount > 0) {
            _affectedUnitIds = new List<int>();
            for (int i = 0; i < _affectedUnitCount; i++) {
                _affectedUnitIds.Add(_packet.ReadInt());
            }
        }

        int _locationsCount = _packet.ReadInt();
        List<Vector3> _locations = null;
        if (_locationsCount > 0) {
            _locations = new List<Vector3>();
            for (int i = 0; i < _locationsCount; i++) {
                _locations.Add(_packet.ReadVector3());
            }
        }

        Enemy e = GameState.instance.GetEnemyByUnitID(_enemyId);
        if (e == null) {
            Debug.Log("Enemy was null on EnemyAbilityFired event!");
            return;
        }

        e.HandleAbility(_ability, _affectedUnitIds, _locations);
    }

    public static void EnemyStateChanged(Packet _packet) {
        if (!ConnectionManager.instance.InGame) {
            return;
        }

        int _enemyId = _packet.ReadInt();
        EnemyStateID _stateId = (EnemyStateID)_packet.ReadInt();

        Enemy e = GameState.instance.GetEnemyByUnitID(_enemyId);
        if (e == null) {
            Debug.Log("Enemy was null on state change packet reception!");
            return;
        }

        e.SetState(_stateId);
    }

    public static void SyncDungeonDetails(Packet _packet) {
        DungeonID _dungeonID = (DungeonID)_packet.ReadInt();
        bool _runStarted = _packet.ReadBool();
        bool _runCompleted = _packet.ReadBool();
        float _timeElapsed = _packet.ReadFloat();
        int _enemiesRequired = _packet.ReadInt();
        int _enemiesKilled = _packet.ReadInt();

        Dictionary<Enemies, bool> _bossesKilled = new Dictionary<Enemies, bool>();
        int _bossesKilledCount = _packet.ReadInt();
        if (_bossesKilledCount > 0) {
            for (int i = 0; i < _bossesKilledCount; i++) {
                _bossesKilled.Add((Enemies)_packet.ReadInt(), _packet.ReadBool());
            }
        }

        Dictionary<RunMedal, float> _medalTimes = new Dictionary<RunMedal, float>();
        int _medalCount = _packet.ReadInt();
        if (_medalCount > 0) {
            for (int i = 0; i < _medalCount; i++) {
                _medalTimes.Add((RunMedal)_packet.ReadInt(), _packet.ReadFloat());
            }
        }

        GameState.instance.DD.Sync(_dungeonID, _runStarted, _runCompleted, _timeElapsed, _enemiesRequired, _enemiesKilled, _bossesKilled, _medalTimes);
    }

    public static void BuffApplied(Packet _packet) {
        int _attachedUnitId = _packet.ReadInt();
        BuffID _buffId = (BuffID)_packet.ReadInt();
        float _maxDuration = _packet.ReadFloat();
        float _durationRemaining = _packet.ReadFloat();

        Unit appliedTo = GameState.instance.GetUnitByUnitID(_attachedUnitId);
        if (appliedTo == null) {
            Debug.Log($"Unit with ID {_attachedUnitId} could not be found so we couldn't apply the buff! Doing nothing!");
            return;
        }

        appliedTo.ApplyBuff(_buffId, _maxDuration, _durationRemaining);
    }

    public static void BuffPurged(Packet _packet) {
        int _attachedUnitId = _packet.ReadInt();
        BuffID _buffId = (BuffID)_packet.ReadInt();

        Unit appliedTo = GameState.instance.GetUnitByUnitID(_attachedUnitId);
        if (appliedTo == null) {
            Debug.Log($"Unit with ID {_attachedUnitId} could not be found so we couldn't purge the buff! Doing nothing!");
            return;
        }

        appliedTo.PurgeBuff(_buffId);
    }
}