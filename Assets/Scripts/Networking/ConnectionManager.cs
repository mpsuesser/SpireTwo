using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager instance;

    public bool InGame { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(gameObject);
        }

        InGame = false;
    }

    private void Start() {
        PreloadManager.instance.Ready(this);
    }

    public void ConnectionRequested(string _username) {
        Debug.Log("Connection requested via the ConnectionManager.");
        Client.instance.ConnectToServer();
    }

    public void ConnectedToGame() {
        InGame = true;
    }
}
