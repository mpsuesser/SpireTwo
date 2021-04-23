using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCastBar : MonoBehaviour
{
    private Enemy root;

    public GameObject barRoot;
    public Image castBar;

    public Color castFinishColor;
    private Color originalColor;

    private EnemyCanvas rootCanvas;

    private void Awake() {
        root = gameObject.GetComponentInParent<Enemy>();
        rootCanvas = gameObject.GetComponentInParent<EnemyCanvas>();

        originalColor = castBar.color;
    }

    private void Start() {
        root.CastStarted += ActivateCastBar;
        rootCanvas.ActiveChanged += CheckBar;

        DisableCastBarObject();
    }

    private void OnDestroy() {
        root.CastStarted -= ActivateCastBar;
        rootCanvas.ActiveChanged -= CheckBar;
    }

    private void ActivateCastBar(AbilityID _abilityID, float _castTime) {
        EnableCastBarObject();

        StartCoroutine(Cast(_castTime));
    }

    private IEnumerator Cast(float _castTime) {
        castBar.color = originalColor;

        float startTime = Time.time;
        float currentTime = startTime;
        float endTime = Time.time + _castTime;

        while (currentTime < endTime) {
            castBar.fillAmount = (currentTime - startTime) / _castTime;
            yield return null;
            currentTime += Time.deltaTime;
        }

        yield return StartCoroutine(FinishedCast());
    }

    private IEnumerator FinishedCast() {
        castBar.color = castFinishColor;
        yield return new WaitForSeconds(0.5f);
        DisableCastBarObject();
    }

    private bool LocalActive { get; set; }
    private void EnableCastBarObject() {
        LocalActive = true;

        CheckBar();
    }

    private void DisableCastBarObject() {
        LocalActive = false;

        CheckBar();
    }

    private void CheckBar() {
        barRoot.SetActive(LocalActive && rootCanvas.ShouldBeActive);
    }
}
