using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class Hotbar : MonoBehaviour {

	[SerializeField] TapController _tapController;
	[SerializeField] EssenceUI _essenceUI;
	[SerializeField] CastOptionsUI _castOptionsUI;
	CombatManager _combatManager;
	List<Hotkey> _hotkeys;
	ShortcutUI _shortcutUI;
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
		_shortcutUI = FindObjectOfType<ShortcutUI>();
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

	void Update() {
		if(Input.GetKeyDown(KeyCode.W)) {
			QuickMove(0, 1);
		} else if(Input.GetKeyDown(KeyCode.A)) {
			QuickMove(-1, 0);
		} else if(Input.GetKeyDown(KeyCode.S)) {
			QuickMove(0, -1);
		} else if(Input.GetKeyDown(KeyCode.D)) {
			QuickMove(1, 0);
		}
	}

	void QuickMove(int x, int y) {
		if(Mathf.Abs(x) + Mathf.Abs(y) > 1) {return;}
		if(_baseUnit == null) {return;}
		if(_baseUnit.isCasting) {return;}

		if(_baseUnit.inCombat) {
			if(_baseUnit.attributes.esCurrent > 0) {
				if(_baseUnit.Move(x, y)) {
					_baseUnit.Cast(1);
					_essenceUI.SetFilledEssence(_baseUnit.attributes.esCurrent);
				}

				if(_activeSpell != null) {
					_activeSpell.ResetTiles();
				}
			}
		} else {
			_baseUnit.Move(x, y);
		}

		if(_activeSpell != null) {
			_activeSpell.ResetTiles();
		}
		
		_castOptionsUI.HideUI();
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

				if(hotkey.spell.essenceCost > _baseUnit.attributes.esCurrent) {
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
			ConfirmCast();
		}
	}

	public void ConfirmCast() {
		bool combatStatus = _baseUnit.inCombat;

		if(combatStatus == true) {
			_baseUnit.Cast(_activeSpell.essenceCost);
		}

		_essenceUI.SetFilledEssence(_baseUnit.attributes.esCurrent);
		_activeSpell.ConfirmSpellCast();
		
		if(_baseUnit.isCasting) {
			_shortcutUI.Disable();
			DisableHotkeys();
		}

		_activeSpell.ResetTiles();
		_castOptionsUI.HideUI();

		/* if(_baseUnit.inCombat == combatStatus) {	// If combat status is unchanged, allow auto recast.
			if(!recast || _activeSpell.essenceCost > _baseUnit.currentEssence) {	// Cancel recast if insufficient essence or recast is disabled.
				_activeSpell.ResetTiles();
				_tapController.image.raycastTarget = true;
				_castOptionsUI.HideUI();
			}
		} else {
			_castOptionsUI.CancelCast();
		}*/
		
	}

	public void Recast(Vector2Int position) {
		if(_activeSpell.essenceCost <= _baseUnit.attributes.esCurrent) {
			Debug.Log("Recast");
			_activeSpell.ResetTiles();
			_castOptionsUI.ShowUI();
			_activeSpell.ShowCastRange();
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void ClearCharges() {
		for(int i = 0; i < _hotkeys.Count; i++) {
			if(_hotkeys[i].spell != null) {
				_hotkeys[i].ClearCharges();
				_hotkeys[i].UpdateName();
				_hotkeys[i].UpdateCost();
			}
		}
	}

	public void UpdateHotkeys() {
		for(int i = 0; i < _hotkeys.Count; i++) {
			if(_hotkeys[i].spell != null) {
				_hotkeys[i].UpdateName();
				_hotkeys[i].UpdateCost();
			}
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void ShowHotkeys() {
		foreach(Hotkey hotkey in _hotkeys) {
			hotkey.gameObject.SetActive(!hotkey.hidden);
		}
		//UpdateHotkeys();
	}

	public void HideHotkeys() {
		foreach(Hotkey hotkey in _hotkeys) {
			hotkey.UpdateCost();
			hotkey.gameObject.SetActive(false);
		}
	}

	public void EnableHotkeys(bool forceEnable = false) {
		foreach(Hotkey hotkey in _hotkeys) {
			if(hotkey.spell != null) {
				if(hotkey.spell.essenceCost <= _baseUnit.attributes.esCurrent || forceEnable) {
					hotkey.Enable();
				}
			}
		}
		//UpdateHotkeys();
	}

	public void DisableHotkeys() {
		foreach(Hotkey hotkey in _hotkeys) {
			hotkey.Disable();
		}
		//UpdateHotkeys();
	}

}
