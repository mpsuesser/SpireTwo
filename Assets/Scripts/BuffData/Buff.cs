using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour {
    public BuffData Data => buffData;

    private BuffData buffData;

    private Image positiveIcon;
    private Image negativeIcon;
    private Text durationText;
    private GameObject tooltipContent;
    private Text tooltipText;

    public void Initialize(BuffID _buffId, float _maxDuration, float _durationRemaining) {
        buffData = BuffMap.instance.GetBuffDataForBuffID(_buffId);
        if (buffData == null) {
            Debug.Log($"Could not initialize buff with ID {_buffId}!");

            Destroy(gameObject);
        }

        positiveIcon = transform.Find("Positive").gameObject.GetComponent<Image>();
        negativeIcon = transform.Find("Negative").gameObject.GetComponent<Image>();
        durationText = transform.Find("Duration").gameObject.GetComponent<Text>();
        tooltipContent = transform.Find("Tooltip").gameObject;
        tooltipText = tooltipContent.GetComponentInChildren<Text>();

        positiveIcon.sprite = buffData.icon;
        negativeIcon.sprite = buffData.icon;
        tooltipText.text = $"{buffData.name}\n\n{buffData.description}";

        tooltipContent.SetActive(false);

        StartCoroutine(DurationCountdown(_maxDuration, _durationRemaining));
    }

    public void HoverEnter() {
        tooltipContent.SetActive(true);
    }

    public void HoverExit() {
        tooltipContent.SetActive(false);
    }

    private IEnumerator DurationCountdown(float _maxDuration, float _durationRemaining) {
        float startTime = Time.time;
        float remaining = _durationRemaining;

        while (remaining > 0f) {
            negativeIcon.fillAmount = (_maxDuration - remaining) / _maxDuration;
            durationText.text = Mathf.Round(remaining).ToString();
            yield return null;
            remaining -= Time.deltaTime;
        }
    }
}
