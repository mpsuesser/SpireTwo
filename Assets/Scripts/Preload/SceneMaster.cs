using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour {
    public static SceneMaster instance; // singleton

    public AudioListener AL;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one SceneMaster instance in scene!");
            return;
        }
        instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);
    }

    private enum scenes {
        PRELOAD = 0,
        MAIN_MENU,
        HERO_SELECTION,
        DUNGEON_PRELOAD,
        GAME_WORLD
    }

    void Start() {
        PreloadManager.instance.Ready(this);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene((int)scenes.MAIN_MENU, LoadSceneMode.Additive);
    }

    public void LoadHeroSelection() {
        SceneManager.UnloadSceneAsync((int)scenes.MAIN_MENU);
        SceneManager.LoadScene((int)scenes.HERO_SELECTION, LoadSceneMode.Additive);
    }

    public void DungeonPreload() {
        SceneManager.UnloadSceneAsync((int)scenes.HERO_SELECTION);
        SceneManager.LoadScene((int)scenes.DUNGEON_PRELOAD, LoadSceneMode.Additive);
    }

    // Temporary fix for needing CC to be awake before moving the AudioListener to it.
    public void Ready(CameraController _cc) {
        LoadGameWorld();
    }

    public void LoadGameWorld() {
        SceneManager.LoadScene((int)scenes.GAME_WORLD, LoadSceneMode.Additive);
        ConnectionManager.instance.ConnectedToGame();

        // Move our Audio Listener to the main camera
        AL.gameObject.transform.SetParent(CameraController.instance.gameObject.transform);

        ClientSend.DungeonLoaded();
    }
}
