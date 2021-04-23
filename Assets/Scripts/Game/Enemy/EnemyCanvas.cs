using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour {
    private Enemy root;

    public GameObject canvas;

    public bool ShouldBeActive { get; private set; }
    public event Action ActiveChanged;

    private void Awake() {
        root = gameObject.GetComponentInParent<Enemy>();

        ShouldBeActive = false;
    }

    private void Start() {
        HeroController.instance.H.UpdatedPositionOrRotation += CheckShowConditions;
    }

    private void OnDestroy() {
        HeroController.instance.H.UpdatedPositionOrRotation -= CheckShowConditions;
    }

    private void CheckShowConditions(Hero _h, Vector3 _heroPos, Quaternion _heroRotation) {
        if (!root.IsDead && (_heroPos - root.gameObject.transform.position).magnitude < Constants.ENEMY_CANVAS_DISTANCE) {
            UpdateCanvasRotation(CameraController.instance.transform.position);
            ShowCanvas();
        } else {
            HideCanvas();
        }
    }

    // Spin our canvas around so it's always facing the camera
    private void UpdateCanvasRotation(Vector3 _cameraPos) {
        canvas.transform.rotation = Quaternion.LookRotation(_cameraPos - canvas.transform.position);
    }

    private void ShowCanvas() {
        if (ShouldBeActive) {
            return;
        }

        ShouldBeActive = true;
        ActiveChanged?.Invoke();
    }

    private void HideCanvas() {
        if (!ShouldBeActive) {
            return;
        }

        ShouldBeActive = false;
        ActiveChanged?.Invoke();
    }
}
