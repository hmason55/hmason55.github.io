using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShortcutUI : MonoBehaviour {

	[SerializeField] Button _menuShortcut;
	[SerializeField] Button _characterShortcut;
	[SerializeField] Button _bagShortcut;
	[SerializeField] Button _endTurnShortcut;
	[SerializeField] BagBehaviour _bagBehaviour;

	CombatManager _combatManager;
	Hotbar _hotbar;

	CastOptionsUI _castOptionsUI;

	void Awake() {
		_combatManager = FindObjectOfType<CombatManager>();
		_hotbar = FindObjectOfType<Hotbar>();
		_castOptionsUI = FindObjectOfType<CastOptionsUI>();
	}

	public void OnMenuShortcut() {

	}

	public void OnCharacterShortcut() {

	}

	public void OnBagShortcut() {
		_bagBehaviour.ToggleUI();
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

		_endTurnShortcut.interactable = false;
		Text text = _endTurnShortcut.GetComponentInChildren<Text>();
		text.color = Color.gray;
	}


}
