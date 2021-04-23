using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Enemy root;
    private EnemyCanvas rootCanvas;

    public GameObject barRoot;
    public Image healthBar;

    private void Awake() {
        root = gameObject.GetComponentInParent<Enemy>();
        rootCanvas = gameObject.GetComponentInParent<EnemyCanvas>();
    }

    private void Start() {
        root.Damaged += UpdateHealthBar;
        rootCanvas.ActiveChanged += ToggleShow;

        ToggleShow();
    }

    private void OnDestroy() {
        root.Damaged -= UpdateHealthBar;
        rootCanvas.ActiveChanged -= ToggleShow;
    }

    private void UpdateHealthBar() {
        healthBar.fillAmount = root.Health / root.MaxHealth;
        healthBar.color = Util.GetHealthBarColor(healthBar.fillAmount);
    }

    private void ToggleShow() {
        barRoot.SetActive(rootCanvas.ShouldBeActive);
    }
}
