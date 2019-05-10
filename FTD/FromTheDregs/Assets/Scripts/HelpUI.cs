using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HelpUI : UIBehaviour {
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;

    void Update() {
        if(Input.GetKeyDown(KeyCode.F10)) {
            base.ShowUI();
        } else if(Input.GetKeyDown(KeyCode.Escape)) {
            base.HideUI();
        }
    }

    public void OnConfirm() {
        Application.OpenURL("https://hmason55.github.io/guide/");
        base.HideUI();
    }
}
