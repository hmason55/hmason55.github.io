using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIBehaviour : MonoBehaviour {

    [SerializeField] protected Text _titleText;
    [SerializeField] protected Button _closeButton;
    [SerializeField] protected GameObject _panel;

    protected bool _hidden = true;

    void Start() {

    }

    public void OnClose() {
        HideUI();
    }

    public void ToggleUI() {
        if(_hidden) {
            ShowUI();
        } else {
            HideUI();
        }
    }

    public void ShowUI() {
        if(_hidden) {
            _hidden = false;
            if(_panel != null) {
                _panel.SetActive(true);
            }
        }
    }

    public void HideUI() {
        if(!_hidden) {
            _hidden = true;
            if(_panel != null) {
                _panel.SetActive(false);
            }
        }
    }
}
