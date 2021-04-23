using System;
using UnityEngine;

public static class Util {
    public static Color GetHealthBarColor(float _healthPercentage) {
        int upperBoundRGBScale = 135; // arbitrary, picked from editor because it looked nice
        float red, green;

        if (Mathf.RoundToInt(_healthPercentage) == 1) { // > 50%
            red = ((1f - _healthPercentage) / .5f) * upperBoundRGBScale; // 0 at 100%, 1 at 50%
            green = upperBoundRGBScale;
        } else {
            red = upperBoundRGBScale;
            green = (_healthPercentage / .5f) * upperBoundRGBScale;
        }

        return new Color(red / 255f, green / 255f, 0f, 163f / 255f); // alpha is also arbitrary
    }

    public static string GetRandomString(String[] _strings, System.Random _RNG) {
        if (_strings.Length == 0) {
            return "";
        }

        return _strings[_RNG.Next(_strings.Length)];
    }

    // Gets the ground position below a location
    private static readonly float MaxDistance = 10f;
    private static readonly int GroundLayerMask = Constants.GROUND_LAYER_MASK;
    public static Vector3 GetGround(Vector3 _origin) {
        RaycastHit[] hits = Physics.RaycastAll(_origin, Vector3.down, MaxDistance, GroundLayerMask);

        if (hits.Length == 0) {
            Debug.Log("Could not get a ground point!");
            return _origin;
        }

        RaycastHit currentClosest = hits[0];
        foreach(RaycastHit hit in hits) {
            if (currentClosest.distance > hit.distance) {
                currentClosest = hit;
            }
        }

        return currentClosest.point;
    }

    public static string SecondsToString(float _time) {
        int seconds = (int)Mathf.Round(_time);
        int minutes = seconds / 60;
        seconds -= minutes * 60;

        return String.Format("{0:00}:{1:00}", minutes, seconds);
    }
}