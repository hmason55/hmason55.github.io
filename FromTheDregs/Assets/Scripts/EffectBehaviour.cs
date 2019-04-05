using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBehaviour : MonoBehaviour {
    [SerializeField] Image _effectImage;
    [SerializeField] Text _effectValue;

    public Image effectImage {
        get {return _effectImage;}
    }
    public Text effectValue {
        get {return _effectValue;}
    }

    public void UpdateEffect(BaseUnit baseUnit, Effect effect, SpriteManager spriteManager) {
        if(_effectImage == null || _effectValue == null) {return;}

        switch(effect.effectType) {
            case Effect.EffectType.Block:
                _effectImage.sprite = spriteManager.statuses.block;
                _effectValue.text = effect.deactivationConditions[Effect.Conditions.BlockDamage].ToString();
            break;
        }

        
    }

    public void ShowEffect() {
        if(_effectImage != null) { _effectImage.enabled = true;}
        if(_effectValue != null) {_effectValue.enabled = true;}
    }

    public void HideEffect() {
        if(_effectImage != null) { _effectImage.enabled = false;}
        if(_effectValue != null) {_effectValue.enabled = false;}
    }
}
