using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBehaviour : StateMachineBehaviour
{
    private static readonly string HelperValueName = "ThingHelper";
    private static readonly string ActionFlagName = "DoingThing";

    private bool InitialHelperValue { get; set; }

    // temp
    private int StateNum;

    private float prevCurrentTime;

    private Coroutine unsetCoroutine;

    // We want to know if a transition has begun before unsetting our DoingThing flag, so we get the helper value, negate it, then store it. If the helper value is the same as what we set it to, we know no other DoingThing action has been transitioned to and we can safely unset DoingThing.
    private void SetInitialHelperValues(Animator animator) {
        StateNum = GameState.instance.GetStateNum;

        // Get current helper flag value
        bool currentHelperValue = animator.GetBool(HelperValueName);
        // Debug.Log($"[SN: {StateNum}] Current helper value was {currentHelperValue}");

        // Negate it to signify a new ActionBehaviour state has been entered
        InitialHelperValue = !currentHelperValue;
        animator.SetBool(HelperValueName, InitialHelperValue);

        // Debug.Log($"[SN: {StateNum}] Set helper value to {InitialHelperValue}");

        // Reset current time which will help us determine if the state has transitioned to itself in OnStateUpdate.
        prevCurrentTime = 0f;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetInitialHelperValues(animator);

        animator.SetBool(ActionFlagName, true);

        float totalTimeOfAnimation = stateInfo.length;
        // Debug.Log($"Total time of animation: {totalTimeOfAnimation}");
        unsetCoroutine = animator.gameObject.GetComponentInParent<Unit>().StartCoroutine(UnsetDoingThing(totalTimeOfAnimation, animator));
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // We get the current time of the clip that's playing to determine if we've transitioned to self. This is a workaround necessary only because a transition to self doesn't re-trigger OnStateEnter.
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length == 0) return;

        float currentTime = clipInfo[0].clip.length * stateInfo.normalizedTime;

        // Debug.Log($"Current: {currentTime} ---- prev: {prevCurrentTime}");
        if (currentTime < prevCurrentTime) {
            // Debug.Log("Transitioned to self! Stopping existing coroutine and retriggering OnStateEnter!");
            animator.gameObject.GetComponentInParent<Unit>().StopCoroutine(unsetCoroutine);
            OnStateEnter(animator, stateInfo, layerIndex);
        }

        prevCurrentTime = currentTime;
    }

    private IEnumerator UnsetDoingThing(float _time, Animator _animator) {
        // Debug.Log($"[SN: {StateNum}] Starting timer, helper value is currently {_animator.GetBool(HelperValueName)}");
        yield return new WaitForSeconds(_time);

        // Debug.Log($"[SN: {StateNum}] Timer ended, helper value is currently {_animator.GetBool(HelperValueName)}");
        // If no other ActionBehaviour has been entered in the time we've been waiting, then go ahead and unset the action flag
        if (_animator.GetBool(HelperValueName) == InitialHelperValue) {
            // Debug.Log($"[SN: {StateNum}] Helper values matched. Unsetting action flag.");
            _animator.SetBool(ActionFlagName, false);
        }
    }
}
