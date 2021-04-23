using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonDetails : MonoBehaviour {
    // Window contents
    public Text dungeonName;
    public Text timeElapsed;
    public GameObject requirementsGroup;
    public Text enemyRequirement;

    // Medal times
    public Text goldMedalTime;
    public Text silverMedalTime;
    public Text bronzeMedalTime;
    private Dictionary<RunMedal, float> medalTimes;

    // Frame images based on current time in dungeon
    public Image medalImage;
    public Sprite goldMedalFrame;
    public Sprite silverMedalFrame;
    public Sprite bronzeMedalFrame;
    public Sprite noMedalFrame;

    private float elapsedTimeOnPreviousSync;
    private float timeOfPreviousSync;
    private bool timerRunning;

    void Awake() {
        elapsedTimeOnPreviousSync = 0f;
        timeOfPreviousSync = 0f;
        timerRunning = false;
    }

    public void Sync(DungeonID _dungeonID, bool _runStarted, bool _runCompleted, float _timeElapsed, int _enemiesRequired, int _enemiesKilled, Dictionary<Enemies, bool> _bossesKilled, Dictionary<RunMedal, float> _medalTimes) {
        dungeonName.text = Names.Dungeon[_dungeonID];
        enemyRequirement.text = $"{_enemiesKilled}/{_enemiesRequired} enemies killed";
        Debug.Log($"Run started: {_runStarted}, completed: {_runCompleted}");

        medalTimes = _medalTimes;
        goldMedalTime.text = Util.SecondsToString(_medalTimes[RunMedal.Gold]);
        silverMedalTime.text = Util.SecondsToString(_medalTimes[RunMedal.Silver]);
        bronzeMedalTime.text = Util.SecondsToString(_medalTimes[RunMedal.Bronze]);

        // bosses todo, similar naming to dungeons
        elapsedTimeOnPreviousSync = _timeElapsed;
        timeOfPreviousSync = Time.time;

        if (_runStarted && !_runCompleted) {
            // This should mean this is the first time we're receiving the message that the run is starting
            if (!timerRunning) {
                // TODO: Play sound.
                StartCoroutine(UpdateTimer());
            }
        } else {
            // This should mean this is the first time we're receiving the message that the run has completed
            if (timerRunning) {
                // TODO: Play sound.
            }

            timerRunning = false;
        }

        // store previouslySyncedTimeElapsed
        // if run is active, start routine to update time periodically from previously synced timeElapsed
    }

    private IEnumerator UpdateTimer() {
        timerRunning = true;

        while (timerRunning) {
            UpdateTimerText();
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    private void UpdateTimerText() {
        float time = elapsedTimeOnPreviousSync + (Time.time - timeOfPreviousSync);
        timeElapsed.text = Util.SecondsToString(time);
        UpdateFrameBorder(time);
    }

    private void UpdateFrameBorder(float _time) {
        if (_time < medalTimes[RunMedal.Gold]) {
            medalImage.sprite = goldMedalFrame;
        } else if (_time < medalTimes[RunMedal.Silver]) {
            medalImage.sprite = silverMedalFrame;
        } else if (_time < medalTimes[RunMedal.Bronze]) {
            medalImage.sprite = bronzeMedalFrame;
        } else {
            medalImage.sprite = noMedalFrame;
        }
    }
}
