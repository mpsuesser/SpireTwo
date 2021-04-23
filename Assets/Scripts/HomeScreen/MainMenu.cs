using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    // Options menu handling
    /*
    public SettingsMenu settingsMenuPrefab;
    private SettingsMenu settingsMenu;

    public InputField usernameText;
    public Transform startCanvas;

    public void OptionsClicked() {
        settingsMenu = Instantiate(settingsMenuPrefab, startCanvas.transform);
        settingsMenu.Close += SettingsClosed;

        gameObject.SetActive(false);
    }

    public void SettingsClosed() {
        settingsMenu = null;

        gameObject.SetActive(true);
    } */

    public void ConnectClicked() {
        ConnectionManager.instance.ConnectionRequested("testy123");
    }

    public void QuitClicked() {
        Application.Quit();
    }
}