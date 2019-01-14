using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadDataUI : MonoBehaviour {

    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] Image background;
    [SerializeField] GameObject scrollView;
    [SerializeField] Transform dataSlotContainer;
    [SerializeField] Text titleText;
    [SerializeField] Button closeButton;
    

    void Awake() {
        HideUI();
    }

    void Start() {
        
    }

    public void ShowUI() {
        background.enabled = true;
        scrollView.SetActive(true);
        dataSlotContainer.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);

        SaveLoadData.Load();
        for(int i = 0; i < SaveLoadData.saveLimit; i++) {
            
            Transform child = dataSlotContainer.GetChild(i);
            DataSlot dataSlot = child.GetComponent<DataSlot>();
            if(i < SaveLoadData.savedPlayerData.Count) {
                dataSlot.AssignPlayerData(SaveLoadData.savedPlayerData[i]);
                child.GetComponent<Button>().interactable = true;
            } else {
                dataSlot.portrait.HideCharacter();
                child.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void HideUI() {
        background.enabled = false;
        scrollView.SetActive(false);
        dataSlotContainer.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }

    public void OnClose() {
        HideUI();
        mainMenuUI.ShowUI();
    }
}
