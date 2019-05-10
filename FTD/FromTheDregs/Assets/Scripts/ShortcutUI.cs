using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShortcutUI : UIBehaviour {

	[SerializeField] Button _helpShortcut;
	[SerializeField] Button _menuShortcut;
	[SerializeField] Button _characterShortcut;
	[SerializeField] Button _bagShortcut;
	[SerializeField] Button _endTurnShortcut;
	HelpUI _helpUI;
	MenuUI _menuUI;

	AttributesUI _attributesUI;
	BagBehaviour _bagBehaviour;


	CombatManager _combatManager;
	Hotbar _hotbar;

	CastOptionsUI _castOptionsUI;

	bool _shortcutsEnabled = true;
	bool _suggestEndTurn = false;

	public BagBehaviour bagBehaviour {
		get {return _bagBehaviour;}
	}

	public AttributesUI attributesUI {
		get {return _attributesUI;}
	}

	new void Awake() {
		base.Awake();
		_combatManager = FindObjectOfType<CombatManager>();
		_hotbar = FindObjectOfType<Hotbar>();
		_castOptionsUI = FindObjectOfType<CastOptionsUI>();
		_attributesUI = FindObjectOfType<AttributesUI>();
		_bagBehaviour = FindObjectOfType<BagBehaviour>();
		_helpUI = FindObjectOfType<HelpUI>();
		_menuUI = FindObjectOfType<MenuUI>();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.T)) {
			if(IsPlayerTurn() && _endTurnShortcut.interactable) {
				OnEndTurnShortcut();
			}
		} else if(Input.GetKeyDown(KeyCode.B)) {
			OnBagShortcut();
		} else if(Input.GetKeyDown(KeyCode.C)) {
			OnCharacterShortcut();
		}
		
		if(!_suggestEndTurn) {
			if(IsPlayerTurn()) {
				if(_hotbar.baseUnit.attributes.currentEssence <= 0) {
					EndTurnButtonSuggest();
				}
			}
		}
	}

	public bool IsPlayerTurn() {
		if(_combatManager == null) {return false;}
		if(_hotbar == null) {return false;}
		if(_combatManager.turnQueue.Length <= 0) {return false;}

		BaseUnit cmUnit = _combatManager.turnQueue.queue[0].baseUnit;
		if(cmUnit.Equals(_hotbar.baseUnit) && _hotbar.baseUnit.playerControlled) {
			return true;
		}

		return false;
	}

	public void OnHelpShortcut() {
		if(_shortcutsEnabled) {
			_helpUI.ToggleUI();
		}
	}

	public void OnMenuShortcut() {
		if(_shortcutsEnabled) {
			_menuUI.ToggleUI();
		}
	}

	public void OnCharacterShortcut() {
		if(_shortcutsEnabled) {
			_attributesUI.ToggleUI();
			_bagBehaviour.HideUI();
		}
	}

	public void OnBagShortcut() {
		if(_shortcutsEnabled) {
			_attributesUI.HideUI();
			_bagBehaviour.ToggleUI();
		}
	}

	public void OnEndTurnShortcut() {
		
		if(IsPlayerTurn()) {
			if(_combatManager.turnQueue.Length > 1) {
				EndTurn();
				_hotbar.DisableHotkeys();
			}
		}
		_combatManager.EndTurn(_hotbar.baseUnit);
	}

	public void BeginTurn() {
		
		EndTurnButtonReset();
		Enable();
		_hotbar.EnableHotkeys();
	}

	public void EndTurnButtonSuggest() {
		ColorBlock cb = _endTurnShortcut.colors;
		cb.normalColor = new Color(0.65f, 1f, 0.65f, 1f);
		cb.highlightedColor = new Color(0.85f, 1f, 0.85f, 1f);
		_endTurnShortcut.colors = cb;
		_suggestEndTurn = true;

		Text text = _endTurnShortcut.GetComponentInChildren<Text>();
		text.color = new Color(0.75f, 1f, 0.75f, 1f);
	}

	void EndTurnButtonReset() {
		_endTurnShortcut.interactable = true;
		ColorBlock cb = _endTurnShortcut.colors;
		cb.normalColor = new Color(200f / 255f, 200f / 255f, 200f / 255f);
		cb.highlightedColor = new Color(1f, 1f, 1f, 1f);
		_endTurnShortcut.colors = cb;

		Text text = _endTurnShortcut.GetComponentInChildren<Text>();
		text.color = Color.white;
	}

	void EndTurnButtonDisable() {
		ColorBlock cb = _endTurnShortcut.colors;
		cb.normalColor = new Color(200f / 255f, 200f / 255f, 200f / 255f);
		//cb.highlightedColor = new Color(1f, 1f, 1f, 1f);
		_endTurnShortcut.colors = cb;

		Text text = _endTurnShortcut.GetComponentInChildren<Text>();
		text.color = Color.gray;
		Disable();
	}

	public void EndTurn() {
		if(_hotbar.activeSpell != null) {
			_castOptionsUI.CancelCast();
		}

		EndTurnButtonDisable();

		_suggestEndTurn = false;
	}

	public void Enable() {
		_shortcutsEnabled = true;
		_menuShortcut.interactable = true;
		_characterShortcut.interactable = true;
		_bagShortcut.interactable = true;
		_endTurnShortcut.interactable = true;
	}

	public void Disable() {
		_shortcutsEnabled = false;
		//_menuShortcut.interactable = false;
		_characterShortcut.interactable = false;
		_bagShortcut.interactable = false;
		_endTurnShortcut.interactable = false;
	}

	public void HideAll() {
		_bagBehaviour.HideUI();
		_attributesUI.HideUI();
	}


}
