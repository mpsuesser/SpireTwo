using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffBar : MonoBehaviour {
    public Buff buffPrefab;

    // We will use this refresh trigger to resize the newly added buffs so that they can correctly use their Aspect Ratio Fitter components in tandem with the BuffBar's HLG which does not control width.
    public GameObject refreshTrigger;

    private Transform bar;

    private List<Buff> activeBuffs;

    void Awake() {
        bar = transform;

        activeBuffs = new List<Buff>();
    }

    public void CreateBuff(BuffID _buffId, float _maxDuration, float _remaining) {
        StartCoroutine(RefreshTrigger());
        Buff b = Instantiate(buffPrefab, bar);
        b.Initialize(_buffId, _maxDuration, _remaining);
        activeBuffs.Add(b);
    }

    private IEnumerator RefreshTrigger() {
        refreshTrigger.SetActive(true);
        yield return null;
        refreshTrigger.SetActive(false);
    }

    public void PurgeBuff(BuffID _buffId) {
        Buff b = activeBuffs.Find(i => i.Data.buffID == _buffId);
        if (b == null) {
            Debug.Log($"Could not find active buff with ID {_buffId} to purge!");
            return;
        }

        activeBuffs.Remove(b);
        Destroy(b.gameObject);
    }
}
