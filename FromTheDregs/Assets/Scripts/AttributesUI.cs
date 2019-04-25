using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AttributesUI : UIBehaviour {

    [SerializeField] Text _attributeNameText;
    [SerializeField] Text _attributeValueText;
    BaseUnit _baseUnit;


    public BaseUnit baseUnit {
        get {return _baseUnit;}
        set {_baseUnit = value;}
    }

    public void ToggleUI() {
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
    }
}
