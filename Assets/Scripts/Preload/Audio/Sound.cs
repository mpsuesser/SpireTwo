using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;

    [Space(10)]
    public bool useStartTime;
    public float startTime;

    [Space(10)]
    public bool useDuration;
    public float duration;

    [Space(10)]
    public bool useFade;
    public float fadeWait;
    public float fadeTime;

    [Space(10)]
    public bool loop;

    [Space(10)]
    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    [Space(10)]
    public AudioMixerGroup group;

    [HideInInspector]
    public AudioSource source;
}
