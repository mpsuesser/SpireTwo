using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    public static CursorManager instance; // singleton

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one CursorManager instance in scene!");
            return;
        }
        instance = this;
        #endregion
    }

    public Texture2D defaultCursorSprite;
    public Texture2D atlasCursorSprite;
    public Texture2D priestessCursorSprite;

    private Dictionary<Heroes, Texture2D> heroCursors;

    private Texture2D currentCursor;

    void Start() {
        ResetCursor();

        heroCursors = new Dictionary<Heroes, Texture2D>() {
            { Heroes.ATLAS, atlasCursorSprite },
            { Heroes.PRIESTESS, priestessCursorSprite }
        };

        PreloadManager.instance.Ready(this);
    }

    public void ResetCursor() {
        SetCursor(defaultCursorSprite);
    }

    public void SetHeroCursor(Heroes _heroType) {
        LockCursor();
        currentCursor = heroCursors[_heroType];
    }

    public void OnGUI() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            float w = 50f;
            float h = 50f;
            float x = (Screen.width / 2) - (w / 2);
            float y = (Screen.height / 2) - (h / 2);
            GUI.DrawTexture(new Rect(x, y, w, h), currentCursor);
        }
    }

    private void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToggleCursorLock() {
        if (Cursor.lockState == CursorLockMode.None) {
            LockCursor();
        } else {
            UnlockCursor();
        }
    }

    private void SetCursor(Texture2D sprite) {
        Cursor.SetCursor(
            sprite,
            new Vector2(sprite.width / 2, sprite.height / 2),
            CursorMode.Auto
        );
    }
}
