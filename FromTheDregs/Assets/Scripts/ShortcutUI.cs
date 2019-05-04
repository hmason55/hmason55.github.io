using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShortcutUI : UIBehaviour {

	[SerializeField] Button _menuShortcut;
	[SerializeField] Button _characterShortcut;
	[SerializeField] Button _bagShortcut;
	[SerializeField] Button _endTurnShortcut;
	[SerializeField] MenuUI _menuUI;

	[SerializeField] AttributesUI _attributesUI;
	[SerializeField] BagBehaviour _bagBehaviour;


	CombatManager _combatManager;
	Hotbar _hotbar;

	CastOptionsUI _castOptionsUI;

	bool _shortcutsEnabled = true;

	new void Awake() {
		base.Awake();
		_combatManager = FindObjectOfType<CombatManager>();
		_hotbar = FindObjectOfType<Hotbar>();
		_castOptionsUI = FindObjectOfType<CastOptionsUI>();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.I)) {
			OnBagShortcut();
		}

		if(Input.GetKeyDown(KeyCode.C)) {
			OnCharacterShortcut();
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
		EndTurn();
		_hotbar.DisableHotkeys();
		_combatManager.EndTurn(_hotbar.baseUnit);
	}

	public void BeginTurn() {
		_endTurnShortcut.interactable = true;
		Text text = _endTurnShortcut.GetComponentInChildren<Text>();
		text.color = Color.white;

		_hotbar.EnableHotkeys();
	}

	public void SuggestEndTurn() {
		ColorBlock cb = _endTurnShortcut.colors;
		cb.normalColor = new Color(0.5f, 1f, 0.5f);
		_endTurnShortcut.colors = cb;
	}

	public void EndTurn() {
		if(_hotbar.activeSpell != null) {
			_castOptionsUI.CancelCast();
		}

		ColorBlock cb = _endTurnShortcut.colors;
		cb.normalColor = new Color(200f / 255f, 200f / 255f, 200f / 255f);
		_endTurnShortcut.colors = cb;

		Disable();
		Text text = _endTurnShortcut.GetComponentInChildren<Text>();
		text.color = Color.gray;
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
		_menuShortcut.interactable = false;
		_characterShortcut.interactable = false;
		_bagShortcut.interactable = false;
		_endTurnShortcut.interactable = false;
	}


}
