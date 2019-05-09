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

	[SerializeField] GameObject _cost;
	[SerializeField] Text _keybindText;

	Spell _spell;

	bool _hidden = false;
	bool _active = true;

	bool _showCastRange = false;
	bool _showEffectRange = false;

	public Spell spell {
		get {return _spell;}
		set {_spell = value;}
	}

	public Spell.Preset preset {
		get {return _preset;}
		set {
			//Debug.Log("NEW SPELL");
			_preset = value;
			_spell = new Spell(_preset);
			_text.text = _spell.spellName;
			gameObject.name = _spell.spellName;
			UpdateKeybind();
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

	public bool active {
		get {return _active;}
		set {_active = value;}
	}

	// Use this for initialization
	void Start () {
		_spell = new Spell(_preset);
		_text.text = _spell.spellName;
		gameObject.name = _spell.spellName;
		
		UpdateName();
		UpdateCost();
		UpdateKeybind();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(_active) {
			EventSystem.current.SetSelectedGameObject(null);
			PreviewCast();
		}
	}

	public void ShowMoreInfo() {
		SpellTooltip tooltip = GameObject.FindObjectOfType<SpellTooltip>();
		if(tooltip != null && _spell != null) {
			tooltip.UpdateTooltip(_spell, _hotbar.baseUnit);
			tooltip.Snap(transform.position.x, transform.position.y);
		}
	}

	public void HideMoreInfo() {
		SpellTooltip tooltip = GameObject.FindObjectOfType<SpellTooltip>();
		if(tooltip != null) {
			tooltip.Reset();
		}
	}
	public void UpdateName() {
		if(_spell != null) {
			if(_spell.chargesRemaining > 0) {
				_text.text = _spell.spellName + " x" + _spell.chargesRemaining;
			} else {
				_text.text = _spell.spellName;
			}
		}
	}

	public void UpdateKeybind() {
		if(_spell != null) {
			_keybindText.text = "[ " + (transform.GetSiblingIndex()+1) + " ]";
		}
	}

	public void UpdateCost() {
		if(_spell != null) {
			for(int i = _cost.transform.childCount-1; i > 0; i--) {
				if(i < _spell.essenceCost) {
					_cost.transform.GetChild(i).gameObject.SetActive(true);
				} else {
					_cost.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}

	public void ClearCharges() {
		if(_spell != null) {
			_spell.chargesRemaining = 0;
		}
	}

	public void PreviewCast() {
		//Debug.Log(_spell.essenceCost + "/" + _hotbar.baseUnit.attributes.currentEssence);
		_spell.SyncWithCaster(_hotbar.baseUnit);
		//_hotbar.tapController.image.raycastTarget = false;
		_hotbar.activeSpell = _spell;
		if(_hotbar.baseUnit.inCombat) {
			_hotbar.essenceUI.PreviewUsage(_hotbar.baseUnit.attributes.currentEssence, _spell.essenceCost);
		}
		_hotbar.castOptionsUI.ShowUI();
		_spell.ShowCastRange();
	}
	
	public void Enable() {
		_active = true;
		Button button = GetComponent<Button>();
		button.interactable = true;
		_text.color = Color.white;
	}

	public void Disable() {
		_active = false;
		Button button = GetComponent<Button>();
		button.interactable = false;
		_text.color = Color.gray;
	}
}
