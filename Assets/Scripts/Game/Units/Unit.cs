using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
    public int ID { get; set; }

    public event Action<AbilityID, float> CastStarted;
    public event Action<BuffID, float, float> BuffApplied;
    public event Action<BuffID> BuffPurged;

    public Animator Anim { get; private set; }
    protected Rigidbody RB { get; private set; }

    protected System.Random RNG;

    protected AudioManager AM;
    protected ProjectileCreator PC;
    protected PrefabManager PM;

    protected virtual void Awake() {
        RB = gameObject.GetComponent<Rigidbody>();
        Anim = GetComponentInChildren<Animator>();

        RNG = new System.Random();
    }

    protected virtual void Start() {
        AM = AudioManager.instance;
        PC = ProjectileCreator.instance;
        PM = PrefabManager.instance;
    }

    #region Ability Handling
    protected delegate void AbilityHandler(AbilityID _ability, List<int> _affectedUnitIds, List<Vector3> _locationsData);
    protected abstract Dictionary<AbilityID, AbilityHandler> AbilityHandlers { get; set; }

    public void HandleAbility(AbilityID _abilityID, List<int> _affectedUnitIds, List<Vector3> _locationsData) {
        AbilityHandler handler;
        if (!AbilityHandlers.TryGetValue(_abilityID, out handler)) {
            Debug.Log($"There was no handler for ability ID {_abilityID}! Doing nothing!");
            return;
        }

        handler(_abilityID, _affectedUnitIds, _locationsData);
    }
    #endregion

    #region Buff Handling
    public void ApplyBuff(BuffID _id, float _maxDuration, float _remaining) {
        BuffApplied?.Invoke(_id, _maxDuration, _remaining);
        Debug.Log("Buff applied!");
    }

    public void PurgeBuff(BuffID _id) {
        BuffPurged?.Invoke(_id);
        Debug.Log("Buff purged!");
    }
    #endregion

    // Trigger the action so that subscribers can be notified (at the time of this writing, specifically for cast bar)
    public void StartCast(AbilityID _abilityID, float _castTime) {
        CastStarted?.Invoke(_abilityID, _castTime);
    }

    #region Lerp Position
    protected IEnumerator LerpPosition(Vector3 _source, Vector3 _dest, float _overTime) {
        float time = 0f;
        Vector3 tempDest;

        while (time < _overTime) {
            tempDest = Vector3.Lerp(_source, _dest, time / _overTime);
            if (this is Hero h) {
                h.UpdatePosition(tempDest, true);
            } else {
                transform.position = tempDest;
            }
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = _dest;
    }
    #endregion
}