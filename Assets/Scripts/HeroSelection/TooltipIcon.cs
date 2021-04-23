using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipIcon : MonoBehaviour
{
    public GameObject tooltipContent;

    public void Start() {
        tooltipContent.SetActive(false);
    }

    public void HoverEnter() {
        tooltipContent.SetActive(true);
    }

    public void HoverExit() {
        tooltipContent.SetActive(false);
    }
}
