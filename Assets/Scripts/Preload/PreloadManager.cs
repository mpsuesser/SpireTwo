using UnityEngine;

public class PreloadManager : MonoBehaviour {
    public static PreloadManager instance;

    private bool audioManagerReady;
    private bool connectionManagerReady;
    private bool clientManagerReady;
    private bool cursorMasterReady;
    private bool threadManagerReady;
    private bool settingsReady;
    private bool sceneMasterReady;
    private bool prefabManagerReady;

    private bool EverythingLoaded {
        get {
            return audioManagerReady
                && connectionManagerReady
                && clientManagerReady
                && cursorMasterReady
                && threadManagerReady
                && settingsReady
                && sceneMasterReady
                && prefabManagerReady;
        }
    }

    private void Awake() {
        instance = this;

        audioManagerReady = false;
        connectionManagerReady = false;
        clientManagerReady = false;
        cursorMasterReady = false;
        threadManagerReady = false;
        settingsReady = false;
        sceneMasterReady = false;
        prefabManagerReady = false;
    }

    public void Ready(AudioManager _am) {
        audioManagerReady = true;
        CheckLoadStatus();
    }

    public void Ready(ConnectionManager _cm) {
        connectionManagerReady = true;
        CheckLoadStatus();
    }

    public void Ready(Client _cm) {
        clientManagerReady = true;
        CheckLoadStatus();
    }

    public void Ready(CursorManager _cc) {
        cursorMasterReady = true;
        CheckLoadStatus();
    }

    public void Ready(ThreadManager _tm) {
        threadManagerReady = true;
        CheckLoadStatus();
    }

    public void Ready(Settings _s) {
        settingsReady = true;
        CheckLoadStatus();
    }

    public void Ready(SceneMaster _sm) {
        sceneMasterReady = true;
        CheckLoadStatus();
    }

    public void Ready(PrefabManager _pm) {
        prefabManagerReady = true;
        CheckLoadStatus();
    }

    public void CheckLoadStatus() {
        if (EverythingLoaded) {
            Debug.Log("Everything is loaded!");
            SceneMaster.instance.LoadMainMenu();
        }
    }
}
