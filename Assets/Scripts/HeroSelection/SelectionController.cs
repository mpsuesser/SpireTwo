using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public Heroes selection;

    public PreviewController previewController;

    public void AtlasSelected() {
        selection = Heroes.ATLAS;

        UpdatePreviewController();
    }

    public void ClearSelection() {
        previewController.ClearPreviews();
    }

    private void UpdatePreviewController() {
        previewController.HeroSelected(selection);
    }

    /* public void AtlasSelected() {
        ClientSend.ChooseHero((int)Heroes.ATLAS);
    }

    public void PriestessSelected() {
        ClientSend.ChooseHero((int)Heroes.PRIESTESS);
    } */
}
