using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour {
    public static ProjectileCreator instance;
    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one ProjectileCreator instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    public ProjectileEffectData aresProjectile;
    public ProjectileEffectData cometProjectile;
    public ProjectileEffectData curseProjectile;
    public ProjectileEffectData cyberProjectile;
    public ProjectileEffectData darkMagicProjectile;
    public ProjectileEffectData demonProjectile;
    public ProjectileEffectData kawaiiProjectile;
    public ProjectileEffectData lightningProjectile;
    public ProjectileEffectData magicProjectile;
    public ProjectileEffectData natureProjectile;
    public ProjectileEffectData priestProjectile;
    public ProjectileEffectData starlightFairyProjectile;
    public ProjectileEffectData succubusProjectile;
    public ProjectileEffectData vampireProjectile;
    public ProjectileEffectData winterProjectile;

    private float Speed = 25f;
    private float ImpactOffset = 0.15f;
    private float DestroyDelay = 4f;
    private float FlashFXDestroyDelay = 2f;
    private float ImpactFXDestroyDelay = 2f;

    public void ShootAtTarget(GameObject _source, GameObject _target, ProjectileEffectData _ped) {
        var flash = Instantiate(_ped.FlashFX, _source.transform.position, _source.transform.rotation);
        flash.transform.localScale = _source.transform.localScale;
        Destroy(flash.gameObject, FlashFXDestroyDelay);

        var projectile = Instantiate(_ped.ProjectileFX, _source.transform.position, _source.transform.rotation);

        projectile.Target = _target;
        projectile.transform.forward = _source.transform.forward;
        projectile.Speed = Speed;
        projectile.ImpactFX = _ped.ImpactFX;
        projectile.ImpactFXDestroyDelay = ImpactFXDestroyDelay;
        projectile.ImpactOffset = ImpactOffset;
        projectile.transform.localScale = _source.transform.localScale;

        var trails = projectile.GetComponentsInChildren<TrailRenderer>();
        foreach (var trail in trails)
            trail.widthMultiplier *= _source.transform.localScale.x;

        Destroy(projectile.gameObject, DestroyDelay);
    }

    public void ShootFireballFromSkeletonToHero(Enemy _e, Hero _h) {
        ShootAtTarget(_e.gameObject, _h.gameObject, aresProjectile);
    }
}
