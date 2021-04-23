using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    public Hero Attached { get; private set; }

    public GameObject Container;

    private Camera C;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one CameraController instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    void Start() {
        Attached = null;
        C = gameObject.GetComponent<Camera>();

        // Subscribe to HeroController's SetHero function so we can attach the camera to that hero.
        HeroController.instance.HeroSet += AttachCameraToHero;

        SceneMaster.instance.Ready(this);
    }

    public void AttachCameraToHero(Hero _h) {
        if (Attached != null) {
            Debug.Log("Attaching camera to new hero even though it was already attached to one!");
        }

        // Set the member which keeps track of the hero this camera is attached to.
        Attached = _h;

        _h.UpdatedPositionOrRotation += UpdatePosition;
    }

    public void UpdateRotation(Vector3 _eulerAngles) {
        Container.transform.rotation = Quaternion.Euler(_eulerAngles);
    }

    private static Vector3 CameraOffset = new Vector3(0f, 4f, -15f);
    public void UpdatePosition(Hero _h, Vector3 _pos, Quaternion _rotation) {
        Container.transform.position = _pos;
    }
}
