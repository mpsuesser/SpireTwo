using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Unit {
    public abstract Enemies Type { get; }
    public EnemyStateID State { get; private set; }
    public bool IsDead => State == EnemyStateID.Dead;

    public event Action Damaged;

    protected override abstract Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    private Coroutine lerpPositionRoutine;
    private Coroutine lerpRotationRoutine;

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }

    protected override void Awake() {
        base.Awake();
    }

    public virtual void Initialize(int _id, EnemyStateID _stateId, float _health, float _maxHealth) {
        ID = _id;
        Health = _health;
        MaxHealth = _maxHealth;

        SetState(_stateId);
    }

    public void SetState(EnemyStateID _stateId) {
        Debug.Log($"Setting state to: {_stateId}");
        State = _stateId;

        Anim.SetInteger("State", (int)_stateId);
    }

    public void SetHealth(float _health) {
        Health = _health;

        if (Damaged != null) {
            Damaged();
        }
    }

    public void OnMouseEnter() {
        // ...
    }

    public void OnMouseExit() {
        // ...
    }

    // _pos: The position to update this hero to.
    // _snap: Should we snap there, or linearly interpolate to smooth out the motion?
    private static readonly float RotationMax = Constants.ENEMY_ROTATION_180_LERP_TIME;
    private static readonly float FixedUpdateFrequency = Constants.SERVER_FIXED_UPDATE_FREQUENCY;
    public void UpdatePositionAndRotation(Vector3 _pos, Quaternion _rotation, bool _snap = false) {
        if (_snap) {
            transform.position = _pos;
            transform.rotation = _rotation;
        } else {
            if (lerpPositionRoutine != null) {
                StopCoroutine(lerpPositionRoutine);
            }

            if (lerpRotationRoutine != null) {
                StopCoroutine(lerpRotationRoutine);
            }

            lerpPositionRoutine = StartCoroutine(LerpPosition(transform.position, _pos, FixedUpdateFrequency));

            lerpRotationRoutine = StartCoroutine(LerpRotation(_rotation, RotationMax, FixedUpdateFrequency));
        }
    }

    private IEnumerator LerpRotation(Quaternion _to, float _timeToDo180, float _fixedUpdateFrequency) {
        float time = 0f;
        while (time < _fixedUpdateFrequency) {
            time += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _to, 180 * (time / _timeToDo180));

            yield return null;
        }
    }
}
