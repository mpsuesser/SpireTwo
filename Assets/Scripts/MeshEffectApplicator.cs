using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshEffectApplicator : MonoBehaviour
{
    public static MeshEffectApplicator instance;
    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one MeshEffectApplicator instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }


    public GameObject fireballCastingEffect;

    public GameObject atlasAxeEffect;
    public Color atlasAxeEffectColor;

    public void ApplyFireballCastEffect(GameObject _obj) {
        ApplyEffectToObject(_obj, fireballCastingEffect);
    }

    public void ApplyAtlasAxeEffect(GameObject _axe) {
        GameObject effect = ApplyEffectToObject(_axe, atlasAxeEffect);
        // UpdateColorOfEffect(effect, atlasAxeEffectColor);
    }

    public GameObject ApplyEffectToObject(GameObject _obj, GameObject _effect) {
        GameObject EffectInstance = Instantiate(_effect);
        EffectInstance.transform.parent = _obj.transform;
        EffectInstance.transform.localPosition = Vector3.zero;
        EffectInstance.transform.localRotation = new Quaternion();

        var meshUpdater = EffectInstance.GetComponent<PSMeshRendererUpdater>();
        meshUpdater.UpdateMeshEffect(_obj);

        return EffectInstance;
    }

    public void UpdateColorOfEffect(GameObject _effect, Color _c) {
        if (_effect == null) return;

        var settingColor = _effect.GetComponent<ME_EffectSettingColor>();
        if (settingColor == null) settingColor = _effect.AddComponent<ME_EffectSettingColor>();
        settingColor.Color = _c;
    }
}
