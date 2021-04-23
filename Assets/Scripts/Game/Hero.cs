using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Unit {
    public abstract Heroes Type { get; protected set; }

    protected override abstract Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    protected bool HeroIsControlledByClient => HeroController.instance.H == this;

    public int OwnerID { get; private set; }
    public float Health { get; private set; }
    public float Mana { get; private set; }
    public float MaxHealth { get; private set; }
    public float MaxMana { get; private set; }

    private bool UpdatedPositionOrRotationFlag;
    public event Action<Hero, Vector3, Quaternion> UpdatedPositionOrRotation;
    public event Action<float, float> UpdatedStatus;

    private IEnumerator lerpRoutine;

    protected override void Awake() {
        UpdatedPositionOrRotationFlag = false;

        base.Awake();
    }

    public void Initialize(int _unitId, int _ownerId, float _health, float _maxHealth, float _mana, float _maxMana) {
        ID = _unitId;
        OwnerID = _ownerId;
        Health = _health;
        MaxHealth = _maxHealth;
        Mana = _mana;
        MaxMana = _maxMana;

        InitializeHero();

        if (UpdatedStatus != null) {
            UpdatedStatus(Health, Mana);
        }
    }

    protected abstract void InitializeHero();

    // Polymorphic - extends Unit.HandleAbility(AbilityID, List<int>)
    public void HandleAbility(AbilityID _abilityID, List<int> _affectedUnits, List<Vector3> _locationsData, float _cooldown) {
        // Tell the ability that the server has registered its press (for cooldown and GCD purposes).
        if (HeroIsControlledByClient) {
            Ability a = AbilityBarManager.instance.CurrentBar.GetAbilityByID(_abilityID);

            if (a != null) {
                a.PressedServer(_cooldown);
            }
        }

        HandleAbility(_abilityID, _affectedUnits, _locationsData);
    }


    #region Status Setters
    public void SetHealth(float _health) {
        Health = _health;

        if (UpdatedStatus != null) {
            UpdatedStatus(Health, Mana);
        }
    }

    public void SetMana(float _mana) {
        Mana = _mana;

        if (UpdatedStatus != null) {
            UpdatedStatus(Health, Mana);
        }
    }

    public void SetMaxHealth(float _maxHealth) {
        MaxHealth = _maxHealth;
    }

    public void SetMaxMana(float _maxMana) {
        MaxMana = _maxMana;
    }
    #endregion

    #region Updating Position and Rotation
    public void UpdateRotation(Vector3 _eulerAngles) {
        transform.eulerAngles = _eulerAngles;

        UpdatedPositionOrRotationFlag = true;
    }

    // _pos: The position to update this hero to.
    // _snap: Should we snap there, or linearly interpolate to smooth out the motion?
    public void UpdatePosition(Vector3 _pos, bool _snap = false) {
        if (_snap) {
            transform.position = _pos;

            UpdatedPositionOrRotationFlag = true;
        } else {
            if (lerpRoutine != null) {
                StopCoroutine(lerpRoutine);
            }

            lerpRoutine = LerpPosition(transform.position, _pos, Constants.SERVER_FIXED_UPDATE_FREQUENCY);
            StartCoroutine(lerpRoutine);
        }
    }

    public void LateUpdate() {
        if (UpdatedPositionOrRotationFlag == true && UpdatedPositionOrRotation != null) {
            UpdatedPositionOrRotation(this, transform.position, transform.rotation);
        }

        UpdatedPositionOrRotationFlag = false;
    }
    #endregion

    #region Animation Helpers
    public void SetRunning(Movement.Direction _running) {
        Anim.SetInteger("Running", (int)_running);
    }

    public void SetJumping() {
        Anim.SetTrigger("Jump");
    }
    #endregion
}
