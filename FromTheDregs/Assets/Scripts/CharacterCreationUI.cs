using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterCreationUI : MonoBehaviour {

    [SerializeField] Image background;
    [SerializeField] Text titleText;
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] Portrait portrait;
    [SerializeField] UnitBehaviour unitPreview;
    [SerializeField] Button closeButton;
    [SerializeField] GameObject controls;
    [SerializeField] InputField nameField;
    [SerializeField] Button startButton;

    bool allowStart = false;

    void Awake() {
        HideUI();
    }

    public void OnClose() {
        HideUI();
        mainMenuUI.ShowUI();
    }

    public void OnNameChanged() {
        if(nameField.text.Length > 0) {
            allowStart = true;
        } else {
            allowStart = false;
        }
        startButton.interactable = allowStart;
    }

    public void OnStart() {
        HideUI();
        CreateDataSlot();
        
        DungeonManager dungeonManager = GameObject.FindObjectOfType<DungeonManager>();
        if(dungeonManager != null) {
            HideUI();
            dungeonManager.Load();
        }
    }

    void CreateDataSlot() {
        PlayerData playerData = new PlayerData();
        playerData.slot = -1;

        Character character = new Character();
        character.name = nameField.text;
        character.location = "The Beginning";
        
        character.faceType = portrait.faceType;
        character.mouthType = portrait.mouthType;
        character.beardType = portrait.beardType;
        character.noseType = portrait.noseType;
        character.hairType = portrait.hairType;
        character.skinColor = portrait.skinColor;
        character.hairColor = portrait.hairColor;

        playerData.character = character;
        PlayerData.current = playerData;

        SaveLoadData.Save();
    }

    public void ShowUI() {
        background.enabled = true;
        titleText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        portrait.gameObject.SetActive(true);
        unitPreview.ShowUnit();
        controls.gameObject.SetActive(true);
        nameField.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        startButton.interactable = allowStart;
    }
    public void HideUI() {
        background.enabled = false;
        titleText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        portrait.gameObject.SetActive(false);
        unitPreview.HideUnit();
        controls.gameObject.SetActive(false);
        nameField.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }
}
