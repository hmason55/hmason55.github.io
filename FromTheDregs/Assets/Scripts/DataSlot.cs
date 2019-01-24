using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DataSlot : MonoBehaviour {
    [SerializeField] Portrait _portrait;
    [SerializeField] Text nameText;
    [SerializeField] Text locationText;

    [SerializeField] LoadDataUI loadDataUI;

    public Portrait portrait {
        get {return _portrait;}
    }
    
    void Awake() {

    }

    public void LoadGame() {
        int ndx = transform.GetSiblingIndex();
        Debug.Log("Loading from slot " + ndx);
        SaveLoadData.Load();
        //SaveLoadData.savedPlayerData[ndx].slot = ndx;
        PlayerData.current = SaveLoadData.savedPlayerData[ndx];

        DungeonManager dungeonManager = GameObject.FindObjectOfType<DungeonManager>();
        if(dungeonManager != null) {
            loadDataUI.HideUI();
            dungeonManager.Load();
        }
    }

    public void AssignPlayerData(PlayerData playerData) {
        _portrait.LoadCharacter(playerData.character);
        nameText.text = playerData.character.name;
        locationText.text = playerData.character.location;
    }

    public void DeletePlayerData() {
        int ndx = transform.GetSiblingIndex();
        SaveLoadData.savedPlayerData.RemoveAt(ndx);
        PlayerData.current = null;
        SaveLoadData.Save();
        Debug.Log("Deleted save slot " + ndx);
        
        loadDataUI.UpdateDataSlots();
    }
}