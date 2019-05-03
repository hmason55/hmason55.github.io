using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AttributesUI : UIBehaviour {

    [SerializeField] Text _subtitleText;
    [SerializeField] Text _attributeNameText;
    [SerializeField] Text _attributeValueText;
    [SerializeField] Portrait _portrait;
    BaseUnit _baseUnit;


    public BaseUnit baseUnit {
        get {return _baseUnit;}
        set {_baseUnit = value;}
    }

    public new void ToggleUI() {
        if(_hidden) {
            base.ShowUI();
            LoadAttributes();
        } else {
            base.HideUI();
        }
    }

    void LoadAttributes() {
        if(_baseUnit == null) {return;}
        if(_baseUnit.attributes == null) {return;}
        
        _titleText.text = _baseUnit.character.name;
        _subtitleText.text = "Level " + _baseUnit.attributes.level.ToString();

        string values = "";
        values += _baseUnit.attributes.strength + "\n";
        values += _baseUnit.attributes.dexterity + "\n";
        values += _baseUnit.attributes.intelligence + "\n";
        values += _baseUnit.attributes.constitution + "\n";
        values += _baseUnit.attributes.wisdom + "\n";
        values += _baseUnit.attributes.charisma + "\n";
        values += "\n";
        values += _baseUnit.attributes.currentHealth + "/" + _baseUnit.attributes.totalHealth + "\n";
        values += _baseUnit.attributes.currentEssence + "/" + _baseUnit.attributes.totalEssence + "\n";

        _attributeValueText.text = values;

        _portrait.LoadCharacter(_baseUnit.character);
    }
}
