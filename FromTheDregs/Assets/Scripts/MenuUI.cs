using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuUI : UIBehaviour {

    [SerializeField] Button _resumeButton;
    [SerializeField] Button _quitButton;

    public void OnResume() {
        base.HideUI();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            ToggleUI();
        }
    }

    public void OnQuit() {
        Application.Quit();
    }

    public new void ToggleUI() {
        if(_hidden) {
            base.ShowUI();
        } else {
            base.HideUI();
        }
    }
}
