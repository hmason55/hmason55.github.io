using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class Hotkey : MonoBehaviour, IPointerClickHandler {


	[SerializeField] Hotbar _hotbar;
	
	[SerializeField] Image _image;
	[SerializeField] Text _text;

	[SerializeField] Spell.Preset _preset;

	Spell _spell;

	bool _showCastRange = false;
	bool _showEffectRange = false;

	public bool showCastRange {
		get {return _showCastRange;}
		set {_showCastRange = value;}
	}

	// Use this for initialization
	void Start () {
		_spell = new Spell(_preset);
		_text.text = _spell.spellName;
		gameObject.name = _spell.spellName;
	}

	public void OnPointerClick(PointerEventData eventData) {
		_spell = new Spell(_hotbar.baseUnit, _preset);
			//_showCastRange = true;
			_hotbar.tapController.image.raycastTarget = false;
			_hotbar.activeSpell = _spell;
			_hotbar.essenceUI.PreviewUsage(_hotbar.baseUnit.currentEssence, _spell.essenceCost);
			_spell.ShowCastRange();
		/* 
		if(!_showCastRange) {
			_showCastRange = true;
			_hotbar.tapController.image.raycastTarget = false;
			_hotbar.activeSpell = _spell;
			_spell.ShowCastRange();
			_hotbar.essenceUI.PreviewUsage(_hotbar.baseUnit.baseEssence, _spell.essenceCost);
		} else {
			_showCastRange = false;
			_hotbar.tapController.image.raycastTarget = true;
			_spell.ResetTiles();
			_hotbar.essenceUI.ResetAll();
		}*/

	}
}
