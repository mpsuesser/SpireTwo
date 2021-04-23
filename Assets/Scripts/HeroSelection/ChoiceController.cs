using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceController : MonoBehaviour
{
    public SelectionController selectionController;

    public void PickPressed() {
        ClientSend.ChooseHero(selectionController.selection);
    }
}
