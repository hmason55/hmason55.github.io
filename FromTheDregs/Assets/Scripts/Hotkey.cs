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

	[SerializeField] Button _moreInfo;

	[SerializeField] Text _costText;

	Spell _spell;

	bool _hidden = false;
	bool _enabled = true;

	bool _showCastRange = false;
	bool _showEffectRange = false;

	public Spell.Preset preset {
		get {return _preset;}
		set {
			_preset = value;
			_spell = new Spell(_preset);
			_text.text = _spell.spellName;
			gameObject.name = _spell.spellName;

			UpdateCost();
		}
	}

	public bool showCastRange {
		get {return _showCastRange;}
		set {_showCastRange = value;}
	}

	public bool hidden {
		get {return _hidden;}
		set {_hidden = value;}
	}

	public bool enabled {
		get {return _enabled;}
		set {_enabled = value;}
	}

	// Use this for initialization
	void Start () {
		_spell = new Spell(_preset);
		_text.text = _spell.spellName;
		gameObject.name = _spell.spellName;

		UpdateCost();
	}

	

	public void OnPointerClick(PointerEventData eventData) {
		if(_enabled) {
			PreviewCast();
		}
	}

	public void ShowMoreInfo() {
		Debug.Log("Show info");
	}

	public void HideMoreInfo() {
		Debug.Log("Hide info");
	}

	public void UpdateCost() {
		_costText.text = _spell.essenceCost.ToString();
	}

	public void PreviewCast() {
		_spell = new Spell(_hotbar.baseUnit, _preset);
		_hotbar.tapController.image.raycastTarget = false;
		_hotbar.activeSpell = _spell;
		_hotbar.essenceUI.PreviewUsage(_hotbar.baseUnit.currentEssence, _spell.essenceCost);
		_hotbar.castOptionsUI.ShowUI();
		_spell.ShowCastRange();
	}
	
	public void Enable() {
		_enabled = true;
		Button button = GetComponent<Button>();
		button.interactable = true;
		_text.color = Color.white;
	}

	public void Disable() {
		_enabled = false;
		Button button = GetComponent<Button>();
		button.interactable = false;
		_text.color = Color.gray;
	}
}
