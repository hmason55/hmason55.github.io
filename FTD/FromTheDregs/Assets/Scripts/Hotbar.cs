using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class Hotbar : MonoBehaviour {

	[SerializeField] EssenceUI _essenceUI;
	[SerializeField] CastOptionsUI _castOptionsUI;
	CombatManager _combatManager;
	List<Hotkey> _hotkeys;
	ShortcutUI _shortcutUI;
	HelpUI _helpUI;
	MenuUI _menuUI;
	BaseUnit _baseUnit;
	Spell _activeSpell;

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
		_menuUI = FindObjectOfType<MenuUI>();
		_helpUI = FindObjectOfType<HelpUI>();
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
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(!_castOptionsUI.hidden) {
				CancelPreview();
			} else {
				if(_shortcutUI.IsPlayerTurn()) {
					if(!_shortcutUI.bagBehaviour.hidden || !_shortcutUI.attributesUI.hidden) {
						if(_helpUI.hidden) {
							_shortcutUI.HideAll();
						}
					} else {
						if(_helpUI.hidden) {
							_menuUI.ToggleUI();
						}
					}
				} else {
					if(_helpUI.hidden) {
						_menuUI.ToggleUI();
					}
				}
				
			}
		} else if(Input.GetKeyDown(KeyCode.Alpha1) && _hotkeys.Count >= 1) {
			if(_hotkeys[0].active && !_baseUnit.isCasting) {
				_hotkeys[0].PreviewCast();
			}
		} else if(Input.GetKeyDown(KeyCode.Alpha2) && _hotkeys.Count >= 2) {
			if(_hotkeys[1].active && !_baseUnit.isCasting) {
				_hotkeys[1].PreviewCast();
			}
		} else if(Input.GetKeyDown(KeyCode.Alpha3) && _hotkeys.Count >= 3) {
			if(_hotkeys[2].gameObject.activeSelf) {
				_hotkeys[2].PreviewCast();
			}
		} else if(Input.GetKeyDown(KeyCode.Alpha4) && _hotkeys.Count >= 4) {

		} else if(Input.GetKeyDown(KeyCode.Alpha5) && _hotkeys.Count >= 5) {

		} else if(Input.GetKeyDown(KeyCode.Alpha6) && _hotkeys.Count >= 6) {

		} else if(Input.GetMouseButtonDown(1)) {
			if(_activeSpell != null) {
				CancelPreview();
			}
		} else if(Input.GetKeyDown(KeyCode.W)) {
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
		if(_combatManager.turnCount < 0) {return;}

		if(_baseUnit.inCombat) {
			if(_baseUnit.attributes.currentEssence > 0) {
				if(_baseUnit.Move(x, y)) {
					_essenceUI.SetFilledEssence(_baseUnit.attributes.currentEssence);
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

		if(_baseUnit != b) {
			_baseUnit = b;
		}
		
		for(int i = 0; i < _hotkeys.Count; i++) {
			if(i < _baseUnit.spells.Count) {
				Hotkey hotkey = _hotkeys[i];
				hotkey.gameObject.SetActive(true);

				//if(newUnit) {
					hotkey.preset = _baseUnit.spells[i];
				//}
				//Debug.Log(hotkey.spell.spellName + ": " + hotkey.spell.essenceCost + "/" + _baseUnit.attributes.currentEssence);
				if(hotkey.spell.essenceCost > _baseUnit.attributes.currentEssence) {
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
		if(!_castOptionsUI.hidden) {
			_castOptionsUI.HideUI();
		}

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

		if(combatStatus == true && _activeSpell != null) {
			_baseUnit.Cast(_activeSpell.essenceCost);
		}

		_essenceUI.SetFilledEssence(_baseUnit.attributes.currentEssence);
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
		if(_activeSpell.essenceCost <= _baseUnit.attributes.currentEssence) {
			//Debug.Log("Recast");
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
				_hotkeys[i].UpdateKeybind();
			}
		}
	}

	public void UpdateHotkeys() {
		for(int i = 0; i < _hotkeys.Count; i++) {
			if(_hotkeys[i].spell != null) {
				_hotkeys[i].UpdateName();
				_hotkeys[i].UpdateCost();
				_hotkeys[i].UpdateKeybind();
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
			hotkey.UpdateKeybind();
			hotkey.gameObject.SetActive(false);
		}
	}

	public void EnableHotkeys(bool forceEnable = false) {
		foreach(Hotkey hotkey in _hotkeys) {
			if(hotkey.spell != null) {
				if(hotkey.spell.essenceCost <= _baseUnit.attributes.currentEssence || forceEnable) {
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
