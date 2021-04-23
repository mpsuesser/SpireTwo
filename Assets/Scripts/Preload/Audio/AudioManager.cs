using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public AudioMixer audioMixer;
    public Sound[] sounds;

    public static AudioManager instance; // singleton

    // Start is called before the first frame update
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds) {
            InitializeSoundWithSource(s, gameObject.AddComponent<AudioSource>());
        }
    }

    private void InitializeSoundWithSource(Sound s, AudioSource src) {
        s.source = src;
        src.clip = s.clip;
        src.outputAudioMixerGroup = s.group;
        src.volume = s.volume;
        src.pitch = s.pitch;
        src.loop = s.loop;
    }

    void Start() {
        PreloadManager.instance.Ready(this);

        // Play("Intro");
    }

    public void PlayWithDelay(string name, float delay) {
        StartCoroutine(Delay(name, delay));
    }

    public void PlayFromUnitWithDelay(string name, float delay, Unit unit) {
        StartCoroutine(Delay(name, delay, unit));
    }

    private IEnumerator Delay(string name, float delay) {
        yield return new WaitForSeconds(delay);

        Play(name);
    }

    private IEnumerator Delay(string name, float delay, Unit unit) {
        yield return new WaitForSeconds(delay);

        PlayFromUnit(name, unit);
    }

    public AudioSource Play(string name, AudioSource _providedSource = null) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning($"Could not find sound {name}");
            return null;
        }

        AudioSource source;
        if (_providedSource == null) {
            source = s.source;
        } else {
            source = _providedSource;

            InitializeSoundWithSource(s, source);
        }

        // Use our volume setting as a multiplier on the sound, so we can adjust the volume of sounds individually in the sounds array relative to each other
        audioMixer.SetFloat("volume", Mathf.Log10(Settings.instance.Volume * s.volume) * 20);

        // Set volume back to normal for sounds that we have faded out in the past
        source.volume = s.volume;

        if (s.useStartTime) {
            source.time = s.startTime;
        }

        if (s.useDuration) {
            StartCoroutine(CutSoundAfterTime(s.source, s.duration));
        }

        if (s.useFade) {
            StartCoroutine(FadeOut(s.source, s.fadeWait, s.fadeTime));
        }

        source.Play();
        return source;
    }

    public AudioSource PlayFromUnit(string name, Unit unit) {
        AudioSource source = unit.gameObject.GetComponent<AudioSource>();
        if (source == null) {
            Debug.Log($"Unit {unit.gameObject.name} did not have an audio source attached!");
            return null;
        }

        return Play(name, source);
    }

    #region CutSoundAfterTime
    private IEnumerator CutSoundAfterTime(AudioSource _source, float _time) {
        yield return new WaitForSeconds(_time);

        _source.Stop();
    }
    #endregion

    #region Fade
    private IEnumerator FadeOut(AudioSource _source, float _wait, float _overTime) {
        yield return new WaitForSeconds(_wait);

        float currentTime = 0f;
        float startVolume = _source.volume;

        while (currentTime < _overTime) {
            currentTime += Time.deltaTime;
            _source.volume = Mathf.Lerp(startVolume, 0f, currentTime / _overTime);
            yield return null;
        }
    }
    #endregion
}
