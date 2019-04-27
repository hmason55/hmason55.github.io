using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] Button loadGameButton;
    [SerializeField] Button newGameButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;
    
    [SerializeField] LoadDataUI loadDataUI;
    [SerializeField] CharacterCreationUI characterCreationUI;

    [SerializeField] Text titleText;
    [SerializeField] Text versionText;

    bool allowNewGame = true;

    void Awake() {
        SaveLoadData.Load();
        if(SaveLoadData.savedPlayerData.Count >= SaveLoadData.saveLimit) {
            allowNewGame = false;
        }
    }

    public void ShowUI() {
        loadGameButton.gameObject.SetActive(true);
        newGameButton.gameObject.SetActive(true);
        newGameButton.interactable = allowNewGame;
        optionsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);
        versionText.gameObject.SetActive(true);
    }

    public void HideUI() {
        loadGameButton.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        versionText.gameObject.SetActive(false);
    }

    public void OnLoadButton() {
        HideUI();
        loadDataUI.ShowUI();
    }

    public void OnStartButton() {
        HideUI();
        characterCreationUI.ShowUI();
    }

    public void OnQuit() {
        Application.Quit();
    }
}
