using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hotbar : MonoBehaviour {

	[SerializeField] TapController _tapController;
	[SerializeField] EssenceUI _essenceUI;
	[SerializeField] CastOptionsUI _castOptionsUI;
	CombatManager _combatManager;
	List<Hotkey> _hotkeys;
	BaseUnit _baseUnit;
	Spell _activeSpell;

	public TapController tapController {
		get {return _tapController;}
		set {_tapController = value;}
	}

	public EssenceUI essenceUI {
		get {return _essenceUI;}
		set {_essenceUI = value;}
	}

	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}

	public Spell activeSpell {
		get {return _activeSpell;}
		set {_activeSpell = value;}
	}

	public CastOptionsUI castOptionsUI {
		get {return _castOptionsUI;}
	}

	void Awake() {
		_combatManager = FindObjectOfType<CombatManager>();
		InitHotkeys();
	}

	void InitHotkeys() {
		_hotkeys = new List<Hotkey>();
		for(int i = 0; i < transform.childCount; i++) {
			Hotkey hotkey = transform.GetChild(i).GetComponent<Hotkey>();
			if(hotkey != null) {
				_hotkeys.Add(hotkey);
			}
		}
	}

	public void SyncUnit(BaseUnit b) {
		bool newUnit = false;
		if(_baseUnit != b) {
			_baseUnit = b;
			newUnit = true;
		}
		for(int i = 0; i < _hotkeys.Count; i++) {
			if(i < _baseUnit.spells.Count) {
				Hotkey hotkey = _hotkeys[i];
				hotkey.gameObject.SetActive(true);

				if(newUnit) {
					hotkey.preset = _baseUnit.spells[i];
				}

				if(hotkey.spell.essenceCost > _baseUnit.currentEssence) {
					hotkey.Disable();
				} else {
					hotkey.Enable();
				}
				hotkey.hidden = false;
			} else {
				Hotkey hotkey = _hotkeys[i];
				hotkey.gameObject.SetActive(false);
				hotkey.hidden = true;
			}
		}
	}

	public void CancelPreview() {
		_tapController.image.raycastTarget = true;
		_activeSpell.ResetTiles();
		_activeSpell.DestroyCastParticles();
		_essenceUI.ResetAll();
	}

	public void ReadyCast(Vector2Int position) {
		if(_activeSpell.requireCastConfirmation) {
			_activeSpell.ResetTiles();
			_activeSpell.ShowEffectRange(position);
		} else {
			_activeSpell.effectOrigin = position;
			ConfirmCast(true);
		}
		//_essenceUI.PreviewUsage(_baseUnit.currentEssence, _activeSpell.essenceCost);
	}

	public void ConfirmCast(bool recast = false) {
		bool combatStatus = _baseUnit.inCombat;

		_activeSpell.ConfirmSpellCast();
		_baseUnit.Cast(_activeSpell.essenceCost);
		_essenceUI.SetFilledEssence(_baseUnit.currentEssence);
		_activeSpell.SyncWithCaster(_baseUnit);
		
		if(_baseUnit.inCombat == combatStatus) {	// If combat status is unchanged, allow auto recast.
			if(!recast || _activeSpell.essenceCost > _baseUnit.currentEssence) {	// Cancel recast if insufficient essence or recast is disabled.
				_activeSpell.ResetTiles();
				_tapController.image.raycastTarget = true;
				_castOptionsUI.HideUI();
			}
		} else {
			_castOptionsUI.CancelCast();
		}
	}

	public void ShowHotkeys() {
		foreach(Hotkey hotkey in _hotkeys) {
			hotkey.gameObject.SetActive(!hotkey.hidden);
		}
	}

	public void HideHotkeys() {
		foreach(Hotkey hotkey in _hotkeys) {
			hotkey.gameObject.SetActive(false);
		}
	}

	public void EnableHotkeys() {
		foreach(Hotkey hotkey in _hotkeys) {
			hotkey.Enable();
		}
	}

	public void DisableHotkeys() {
		foreach(Hotkey hotkey in _hotkeys) {
			hotkey.Disable();
		}
	}

}
