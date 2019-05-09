using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemTooltip : MonoBehaviour {
    
    [SerializeField] Text _titleText;
    [SerializeField] Text _categoryText;
    [SerializeField] Image _itemImage;
    [SerializeField] Text _attributesText;
    [SerializeField] Text _spellsText;
    [SerializeField] Text _descriptionText;
    [SerializeField] Text _valueText;

    RectTransform _rectTransform;

    void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Snap(float x, float y) {

        if(x < Screen.width/2.5f) {
            x += Screen.width/8.5f;
        } else if(x < Screen.width/1.5f) {
            x -= Screen.width/8.5f;
        } else if(x < Screen.width/1.25f) {
            x += Screen.width/8.5f;
        } else {
            x -= Screen.width/8.5f;
        }
        

        if(y < Screen.height/4f) {
            y += Screen.height/4f;
        } else {
            y += Screen.height/12f;
        }

        transform.position = new Vector3(x, y);
    }

    public void Reset() {
       _rectTransform.anchoredPosition = new Vector3(-2400f, -2400f); 
    }

    public void UpdateTooltip(BaseItem item) {
        if(item == null) {return;}

        _titleText.text = item.name;
        _categoryText.text = item.CategoryToString();
        _attributesText.text = item.AttributesToString();
        _spellsText.text = item.SpellsToString();
        _itemImage.sprite = item.sprite;
        _descriptionText.text = item.description;
        _valueText.text = (item.value * item.quantity).ToString();
        
    }

}
