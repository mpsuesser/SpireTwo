using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {
    private Animator anim;
    public string firstAttackAnimationName;
    public string secondAttackAnimationName;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void PlayAttack1() {
        anim.Play(firstAttackAnimationName);
    }

    public void PlayAttack2() {
        anim.Play(secondAttackAnimationName);
    }
}
