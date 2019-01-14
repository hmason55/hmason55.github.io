using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DataSlot : MonoBehaviour {
    [SerializeField] Portrait _portrait;
    [SerializeField] Text nameText;
    [SerializeField] Text locationText;

    public Portrait portrait {
        get {return _portrait;}
    }
    
    void Awake() {

    }

    public void AssignPlayerData(PlayerData playerData) {
        _portrait.LoadCharacter(playerData.character);
        nameText.text = playerData.character.name;
        locationText.text = playerData.character.location;
    }
}