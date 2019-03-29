using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemTooltip : MonoBehaviour {
    
    [SerializeField] Text titleText;
    [SerializeField] Text categoryText;
    [SerializeField] Image itemImage;
    [SerializeField] Text attributesText;
    [SerializeField] Text spellsText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text valueText;

    RectTransform _rectTransform;

    void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Snap(float x, float y) {

        if(x > Screen.width - 500f) {
            x -= 640f;
        }

        if(y < 360f) {
            y = 360f;
        }

        transform.position = new Vector3(x + 224f, y + 80f);
    }

    public void Reset() {
       _rectTransform.anchoredPosition = new Vector3(-2400f, -2400f); 
    }

    public void UpdateTooltip(BaseItem item) {
        if(item == null) {return;}

        titleText.text = item.name;
        categoryText.text = item.CategoryToString();
        attributesText.text = item.AttributesToString();
        spellsText.text = item.SpellsToString();
        itemImage.sprite = item.sprite;
        descriptionText.text = item.description;
        valueText.text = (item.value * item.quantity).ToString();
        
    }

}
