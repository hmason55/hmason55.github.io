using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpellTooltip : MonoBehaviour {
    
    [SerializeField] Text _titleText;
    [SerializeField] Image _spellImage;
    [SerializeField] Text _targetingAttributesText;
    [SerializeField] Text _targetingValuesText;
    [SerializeField] Text _damageAttributesText;
    [SerializeField] Text _damageValuesText;
    [SerializeField] Text _descriptionText;
    [SerializeField] GameObject _cost;

    RectTransform _rectTransform;

    void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Snap(float x, float y) {

        if(x < Screen.width/2.5f) {
            x += Screen.width/8.5f;
        } else if(x < Screen.width/1.5f) {
            x += Screen.width/8.5f;
        } else if(x < Screen.width/1.25f) {
            x += Screen.width/8.5f;
        } else {
            x -= Screen.width/8.5f;
        }
        

        if(y < Screen.height/4f) {
            y += Screen.height/2.5f;
        } else {
            y += Screen.height/12f;
        }

        transform.position = new Vector3(x, y);
    }

    public void Reset() {
       _rectTransform.anchoredPosition = new Vector3(-2400f, -2400f); 
    }

    public void UpdateTooltip(Spell spell, BaseUnit baseUnit) {
        if(spell == null) {return;}

        _titleText.text = spell.spellName;
        _targetingAttributesText.text = spell.TargetingAttributesToString();
        _targetingValuesText.text = spell.TargetingValuesToString();
        _damageAttributesText.text = spell.PotencyAttributesToString(baseUnit.bag, baseUnit.attributes);
        _damageValuesText.text = spell.PotencyValuesToString(baseUnit.bag, baseUnit.attributes);

        // Update the cost
        for(int i = _cost.transform.childCount-1; i > 0; i--) {
            if(i < spell.essenceCost) {
                _cost.transform.GetChild(i).gameObject.SetActive(true);
            } else {
                _cost.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        //_spellImage.sprite = item.sprite;
        //_descriptionText.text = spell.description;
        _descriptionText.text = "Spell description.";
        //valueText.text = (item.value * item.quantity).ToString();
    }

}
