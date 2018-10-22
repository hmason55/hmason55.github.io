using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class Hotkey : MonoBehaviour, IPointerClickHandler {


	[SerializeField] Hotbar _hotbar;
	[SerializeField] TapController _tapController;
	[SerializeField] Image _image;
	[SerializeField] Text _text;

	[SerializeField] Spell.Preset _preset;

	Spell _spell;

	bool _showCastRange = false;
	bool _showEffectRange = false;

	// Use this for initialization
	void Start () {
		_spell = new Spell(_preset);
		_text.text = _spell.spellName;
		gameObject.name = _spell.spellName;
	}

	public void OnPointerClick(PointerEventData eventData) {
		_spell = new Spell(_hotbar.baseUnit, _preset);
		if(!_showCastRange) {
			_showCastRange = true;
			_tapController.image.raycastTarget = false;
			_hotbar.activeSpell = _spell;
			_spell.ShowCastRange();
			
		} else {
			_showCastRange = false;
			_tapController.image.raycastTarget = true;
			_spell.ResetTiles();
		}

	}
}
