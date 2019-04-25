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

    public void UpdateEffect(BaseUnit baseUnit, Effect effect) {
        if(_effectImage == null || _effectValue == null) {return;}

        switch(effect.effectType) {
        
            case Effect.EffectType.Bleed:
                _effectImage.sprite = AssetReference.sprites.statuses.bleed;
                _effectValue.text = effect.deactivationConditions[Effect.Conditions.DurationExpire].ToString();
            break;

            case Effect.EffectType.Block:
                _effectImage.sprite = AssetReference.sprites.statuses.block;
                _effectValue.text = effect.deactivationConditions[Effect.Conditions.BlockDamage].ToString();
            break;

            case Effect.EffectType.DisplayHealth:
                _effectImage.sprite = AssetReference.sprites.statuses.health;
                _effectValue.text = effect.currentHealth.ToString();
            break;

            case Effect.EffectType.Poison:
                _effectImage.sprite = AssetReference.sprites.statuses.poison;
                _effectValue.text = effect.deactivationConditions[Effect.Conditions.DurationExpire].ToString();
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
